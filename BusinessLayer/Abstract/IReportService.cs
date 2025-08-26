using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IReportService
    {
        List<Report> GetReports();
        void AddReport(Report Report);
        Report? GetByID(int id);
        void DeleteReport(Report Report);
        void UpdateReport(Report Report);
    }
}
