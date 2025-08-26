using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.DTOs;
using BusinessLayer.Helper;
using BusinessLayer.Utils;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class IntersectionChangeRequestManager : IIntersectionChangeRequestService
    {
        private readonly IIntersectionChangeRequestDal _reqDal;
        private readonly IIntersectionDal _intersectionDal;
        private readonly IMapper _mapper;
        private static readonly JsonSerializerOptions _jsonOpts = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        public IntersectionChangeRequestManager(
            IIntersectionChangeRequestDal reqDal,
            IIntersectionDal intersectionDal,
            IMapper mapper)
        {
            _reqDal = reqDal;
            _intersectionDal = intersectionDal;
            _mapper = mapper;
        }

        public void CreateUpdateRequest(Intersection entity, string payloadJson, int requestedByUserId)
        {
            var req = new IntersectionChangeRequest
            {
                IntersectionID = entity.IntersectionID,
                ChangeType = ChangeType.Update,
                PayloadJson = payloadJson,
                RequestedByUserId = requestedByUserId,
                SnapshotUpdatedAt = entity.UpdatedAt
            };
            _reqDal.Insert(req);
        }

        public void CreateCreateRequest(string payloadJson, int requestedByUserId)
        {
            var req = new IntersectionChangeRequest
            {
                ChangeType = ChangeType.Create,
                PayloadJson = payloadJson,
                RequestedByUserId = requestedByUserId,
                SnapshotUpdatedAt = DateTime.UtcNow
            };
            _reqDal.Insert(req);
        }

        public void CreateDeleteRequest(Intersection entity, int requestedByUserId)
        {
            var req = new IntersectionChangeRequest
            {
                IntersectionID = entity.IntersectionID,
                ChangeType = ChangeType.Delete,
                PayloadJson = JsonSerializer.Serialize(new { entity.IntersectionID }),
                RequestedByUserId = requestedByUserId,
                SnapshotUpdatedAt = entity.UpdatedAt
            };
            _reqDal.Insert(req);
        }
        public List<IntersectionChangeRequest> GetPending()
        {
            return _reqDal.List(r => r.Status == ApprovalStatus.Pending, r => r.RequestedBy, r => r.Intersection)
                          .OrderByDescending(r => r.RequestedAt)
                          .ToList();
        }

        public IntersectionChangeRequest? Get(int id)
            => _reqDal.Get(r => r.Id == id);

        public void Approve(int id, int adminUserId)
        {
            var req = _reqDal.Get(r => r.Id == id);
            if (req == null || req.Status != ApprovalStatus.Pending)
                throw new InvalidOperationException("Talep bulunamadı veya beklemede değil.");

            if (req.ChangeType == ChangeType.Update)
            {
                var dto = JsonSerializer.Deserialize<IntersectionUpdateDto>(req.PayloadJson, _jsonOpts)
                          ?? throw new InvalidOperationException("Geçersiz payload.");

                var entity = _intersectionDal.Get(x => x.IntersectionID == req.IntersectionID, x => x.Locations, x => x.Substructure, x => x.IntersectionImages);
                if (entity == null) throw new InvalidOperationException("Kayıt bulunamadı.");

                if (entity.UpdatedAt != req.SnapshotUpdatedAt)
                    throw new InvalidOperationException("Kayıt talep sonrası değişmiş (conflict).");


                _mapper.Map(dto, entity);


                if (dto.Substructure != null)
                {
                    if (entity.Substructure == null)
                        entity.Substructure = _mapper.Map<Substructure>(dto.Substructure);
                    else
                        _mapper.Map(dto.Substructure, entity.Substructure);
                }

                var incoming = dto.Locations ?? new List<LocationDto>();
                var keepIds = incoming.Where(x => x.LocationID.HasValue)
                                      .Select(x => x.LocationID!.Value)
                                      .ToHashSet();

                var toRemove = entity.Locations
                    .Where(e => !keepIds.Contains(e.LocationID))
                    .ToList();
                foreach(var rem in toRemove)
                    entity.Locations.Remove(rem);

                foreach (var l in incoming)
                {
                    if (l.LocationID.HasValue)
                    {
                        var ex = entity.Locations.FirstOrDefault(x => x.LocationID == l.LocationID.Value);
                        if (ex != null) _mapper.Map(l, ex);
                    }
                    else
                    {
                        entity.Locations.Add(_mapper.Map<Location>(l));
                    }
                }
                if(dto.Images != null)
                {
                    if (dto.Images.DeleteIds?.Count > 0)
                    {
                        var dels = entity.IntersectionImages
                            .Where(img => dto.Images.DeleteIds.Contains(img.IntersectionID))
                            .ToList();
                        foreach (var d in dels)
                        {
                            entity.IntersectionImages.Remove(d);
                        }
                    }

                    if (dto.Images.AddStagingPaths?.Count > 0)
                    {
                        foreach (var sp in dto.Images.AddStagingPaths)
                        {
                            var finalRelPath = ImageStagingHelper.MoveFromStaging(sp);
                            entity.IntersectionImages.Add(new IntersectionImage { ImagePath = finalRelPath });
                        }
                    }
                }

                entity.UpdatedAt = DateTime.Now;
                entity.IntersectionStatus = true;
                _intersectionDal.Update(entity);
            }
            else if (req.ChangeType == ChangeType.Create)
            {
                var dto = JsonSerializer.Deserialize<IntersectionUpdateDto>(req.PayloadJson, _jsonOpts)
                          ?? throw new InvalidOperationException("Geçersiz payload.");

                var entity = _mapper.Map<Intersection>(dto);
                entity.UpdatedAt = DateTime.Now;

                if (dto.Substructure != null)
                    entity.Substructure = _mapper.Map<Substructure>(dto.Substructure);

                entity.Locations = (dto.Locations ?? new List<LocationDto>())
                    .Select(l => _mapper.Map<Location>(l))
                    .ToList();
                entity.IntersectionStatus = true;
                _intersectionDal.Insert(entity);
            }
            else if (req.ChangeType == ChangeType.Delete)
            {
                var entity = _intersectionDal.Get(x => x.IntersectionID == req.IntersectionID, x => x.IntersectionImages, x => x.Locations)
                             ?? throw new InvalidOperationException("Kayıt bulunamadı.");

                entity.IntersectionStatus = false;
                entity.UpdatedAt = DateTime.UtcNow;
                _intersectionDal.Update(entity);
            }

            req.Status = ApprovalStatus.Approved;
            req.ReviewedByUserId = adminUserId;
            req.ReviewedAt = DateTime.Now;
            _reqDal.Update(req);
        }

        public void Reject(int id, int adminUserId, string? note)
        {
            var req = _reqDal.Get(r => r.Id == id);
            if (req == null || req.Status != ApprovalStatus.Pending)
                throw new InvalidOperationException("Talep bulunamadı veya beklemede değil.");

            req.Status = ApprovalStatus.Rejected;
            req.ReviewNote = note;
            req.ReviewedByUserId = adminUserId;
            req.ReviewedAt = DateTime.Now;
            _reqDal.Update(req);
        }

        public PendingChangeDetailVM GetRequestDetailWithDiff(int requestId)
        {
            var req = _reqDal.Get(r => r.Id == requestId, r => r.RequestedBy, r => r.Intersection);
            

            if (req == null) throw new InvalidOperationException("Talep bulunamadı.");

            var detail = new PendingChangeDetailVM
            {
                RequestId = req.Id,
                ChangeType = req.ChangeType,
                IntersectionID = req.IntersectionID,
                IntersectionTitle = req.Intersection?.Title ?? (req.ChangeType == ChangeType.Create ? "(Yeni kayıt)" : "-"),
                RequestedAt = req.RequestedAt,
                RequestedByUserName = req.RequestedBy?.UserName ?? req.RequestedByUserId.ToString()
            };

            if (req.ChangeType == ChangeType.Update)
            {
                IntersectionUpdateDto proposed;
                try
                {
                    proposed = JsonSerializer.Deserialize<IntersectionUpdateDto>(req.PayloadJson, _jsonOpts)
                               ?? new IntersectionUpdateDto();
                }
                catch
                {
                    proposed = new IntersectionUpdateDto();
                }

                var entity = _intersectionDal.Get(x => x.IntersectionID == req.IntersectionID,
                                 x => x.Substructure, x => x.Locations, x => x.IntersectionImages);

                if (entity != null)
                {
                    var current = _mapper.Map<IntersectionUpdateDto>(entity);

                    ChangeDiffBuilder.DiffScalars(current, proposed, detail.ScalarDiffs);
                    ChangeDiffBuilder.DiffSubstructure(current.Substructure, proposed.Substructure, detail.SubstructureDiffs);
                    ChangeDiffBuilder.DiffLocations(entity.Locations, proposed.Locations ?? new List<LocationDto>(), detail.LocationDiffs);
                    ChangeDiffBuilder.DiffImages(entity.IntersectionImages, proposed.Images, detail.ImageDiffs);

                    foreach (var d in detail.ScalarDiffs)
                        d.Label = DisplayNameResolver.Resolve(typeof(IntersectionUpdateDto), d.Path);

                    foreach (var d in detail.SubstructureDiffs)
                        d.Label = DisplayNameResolver.Resolve(typeof(IntersectionUpdateDto), d.Path);

                    foreach (var lc in detail.LocationDiffs)
                        foreach (var d in lc.FieldDiffs)
                            d.Label = DisplayNameResolver.Resolve(typeof(LocationDto), d.Path);
                }
            }

            return detail;
        }
    }
}
