using BusinessLayer.DTOs;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IIntersectionChangeRequestService
    {
        void CreateUpdateRequest(Intersection entity, string payloadJson, int requestedByUserId);
        void CreateCreateRequest(string payloadJson, int requestedByUserId);
        void CreateDeleteRequest(Intersection entity, int requestedByUserId);
        List<IntersectionChangeRequest> GetPending();
        IntersectionChangeRequest? Get(int id);
        PendingChangeDetailVM GetRequestDetailWithDiff(int requestId);
        void Approve(int id, int adminUserId);
        void Reject(int id, int adminUserId, string? note);
    }
}
