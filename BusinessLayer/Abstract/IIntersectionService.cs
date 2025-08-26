using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IIntersectionService
    {
        List<Intersection> GetList();
        List<Intersection> GetDeactives();
        void AddIntersection(Intersection intersection);
        Intersection? GetByID(int id);
        Intersection? GetWithSubandReport(int id);
        void DeleteIntersection(Intersection intersection);
        void UpdateIntersection(Intersection intersection);
        bool IsKkcIdInUse(int id);
        bool IsKkcIdInUse(int Iid, int kkcid);
    }
}
