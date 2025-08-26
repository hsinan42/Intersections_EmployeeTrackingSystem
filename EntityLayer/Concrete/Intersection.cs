using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Intersection
    {
        [Key]
        public int IntersectionID { get; set; }
        public int KkcID { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
        public bool IntersectionStatus { get; set; } = true;
        public string RoadName { get; set; } = null!;
        public string BondedOrganisation { get; set; } = null!;
        public int? CamCount { get; set; }
        public int? LoopCount { get; set; }
        public int? GroupCount { get; set; }
        public string? SerialNumber { get; set; }
        public string? CpuHex { get; set; }
        public string? DriverHex { get; set; }
        public DeviceType DeviceType { get; set; }
        public bool PedButton { get; set; }
        public bool UPS { get; set; }
        public int? UserID { get; set; }
        public User User { get; set; } = null!;
        public List<IntersectionImage> IntersectionImages { get; set; } = new();
        public List<Location> Locations { get; set; } = new();
        public List<Report> Reports { get; set; } = new();
        public Substructure Substructure { get; set; } = null!;
    }
}
