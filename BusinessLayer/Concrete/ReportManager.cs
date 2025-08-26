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
    public class ReportManager : IReportService
    {
        private readonly IReportDal _reportDal;

        public ReportManager(IReportDal reportDal)
        {
            _reportDal = reportDal;
        }

        public void AddReport(Report Report)
        {
            _reportDal.Insert(Report);
        }

        public void DeleteReport(Report Report)
        {
            _reportDal.Delete(Report);
        }

        public Report? GetByID(int id)
        {
            return _reportDal.Get(x => x.ReportID == id, x => x.intersection, x => x.User);
        }

        public List<Report> GetReports()
        {
            return _reportDal.List(x => x.intersection, x => x.User);
        }

        public void UpdateReport(Report Report)
        {
            _reportDal.Update(Report);
        }
    }
}
