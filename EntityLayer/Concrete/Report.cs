using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Report
    {
        [Key]
        public int ReportID { get; set; }
        public DateTime ReportDate { get; private set; } = DateTime.Now;
        public string ReportName { get; set; } = null!;
        public string ReportDescription { get; set; } = null!;
        public int IntersectionID { get; set; }
        public Intersection intersection { get; set; } = null!;
        public int? UserID { get; set; }
        public User User { get; set; } = null!;
    }
}
