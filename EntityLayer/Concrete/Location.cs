using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string? Address { get; set; }
        public string Latitude { get; set; } = null!;
        public string Longitude { get; set; } = null!;
        public int IntersectionID { get; set; }
        public Intersection intersection { get; set; } = null!;
    }
}
