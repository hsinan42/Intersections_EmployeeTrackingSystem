using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IImageService
    {
        List<IntersectionImage> GetList();
        void AddImage(IntersectionImage intersectionImage);
        IntersectionImage? GetByID(int id);
        void DeleteImage(IntersectionImage intersectionImage);
        void UpdateImage(IntersectionImage intersectionImage);
    }
}
