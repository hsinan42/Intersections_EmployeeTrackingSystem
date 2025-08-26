using EntityLayer.Concrete;
using EntityLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace Intersections_EmployeeTrackingSystem.Models
{
    public class DetailsVM
    {
        public int IntersectionID { get; set; }
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IntersectionStatus { get; set; }
        public int KkcID { get; set; }
        public string? RoadName { get; set; }
        public string? BondedOrganisation { get; set; }
        public int? CamCount { get; set; }
        public int? LoopCount { get; set; }
        public int? GroupCount { get; set; }
        public string? SerialNumber { get; set; }
        public string? CpuHex { get; set; }
        public string? DriverHex { get; set; }
        [Display(Name = "Cihaz Tipi")]
        public DeviceType DeviceType { get; set; } = DeviceType.Unknown;
        public bool PedButton { get; set; }
        public bool UPS { get; set; }
        public DetailsSubVM? Subtructure { get; set; } = new();
        public List<DetailsRepVM> Reports { get; set; } = new();
        public List<DetailsLocVM> Locations { get; set; } = new();
        public List<IntersectionImage> IntersectionImages { get; set; } = new();
        //public EmployeeViewModel? Employee { get; set; } = new();

    }

    public class DetailsLocVM
    {
        public int LocationID { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Address { get; set; }

    }
    public class DetailsSubVM
    {
        public int SubstructureID { get; set; }
        public string? SubstructureBuilder { get; set; }
        public DateTime? SubstructureStartDate { get; set; }
        public DateTime? SubstructureFinishDate { get; set; }
    }
    public class DetailsRepVM
    {
        public int ReportID { get; set; }
        public DateTime ReportDate { get; set; }
        public string? ReportName { get; set; }
        public string? ReportDescription { get; set; }
        public int IntersectionID { get; set; }
        public int UserID { get; set; }
        public string? UserName { get; set; }

    }
}
