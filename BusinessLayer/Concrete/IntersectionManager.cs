using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class IntersectionManager : IIntersectionService
    {
        private readonly IIntersectionDal _intersectionDal;
        public IntersectionManager(IIntersectionDal intersectionDal)
        {
            _intersectionDal = intersectionDal;
        }
        public void AddIntersection(Intersection intersection)
        {
            _intersectionDal.Insert(intersection);
        }

        public void DeleteIntersection(Intersection intersection)
        {
            _intersectionDal.Delete(intersection);
        }

        public Intersection? GetByID(int id)
        {
            return _intersectionDal.Get(x => x.IntersectionID == id, x => x.User, x => x.Locations, x => x.IntersectionImages);
        }
        public Intersection? GetWithSubandReport(int id)
        {
            return _intersectionDal.Get(
                x => x.IntersectionID == id,
                q => q.Include(x => x.User)
                      .Include(x => x.Locations)
                      .Include(x => x.IntersectionImages)
                      .Include(x => x.Substructure)
                      .Include(x => x.Reports)
                          .ThenInclude(r => r.User)
            );
        }
        public List<Intersection> GetDeactives()
        {
            return _intersectionDal.List(x => x.User, x => x.IntersectionImages, x => x.Locations)
                .Where(x => x.IntersectionStatus == false).ToList();
        }

        public List<Intersection> GetList()
        {
            return _intersectionDal.List(x => x.User, x => x.IntersectionImages, x => x.Locations)
                .Where(x => x.IntersectionStatus == true).ToList();
        }

        public void UpdateIntersection(Intersection intersection)
        {
            _intersectionDal.Update(intersection);
        }

        public bool IsKkcIdInUse(int id)
        {
            return _intersectionDal.Any(x => x.KkcID == id);
        }

        public bool IsKkcIdInUse(int Iid, int kkcid)
        {
            return _intersectionDal.Any(x => x.KkcID == kkcid && x.IntersectionID != Iid);
        }

        public List<Intersection> GetwithReportsList()
        {
            return _intersectionDal.List(x => x.User, x => x.Reports)
                .Where(x => x.IntersectionStatus == true).ToList();
        }
    }
}
