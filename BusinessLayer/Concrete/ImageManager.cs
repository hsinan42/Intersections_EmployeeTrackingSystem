using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ImageManager : IImageService
    {
        private readonly IImageDal _imageDal;

        public ImageManager(IImageDal imageDal)
        {
            _imageDal = imageDal;
        }
        public void AddImage(IntersectionImage intersectionImage)
        {
            _imageDal.Insert(intersectionImage);
        }

        public void DeleteImage(IntersectionImage intersectionImage)
        {
            _imageDal.Delete(intersectionImage);
        }

        public IntersectionImage? GetByID(int id)
        {
            return _imageDal.Get(x => x.IntersectionID == id);
        }

        public List<IntersectionImage> GetList()
        {
            return _imageDal.List();
        }

        public void UpdateImage(IntersectionImage intersectionImage)
        {
            _imageDal.Update(intersectionImage);
        }
    }
}
