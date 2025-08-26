using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class IntersectionImage
    {
        [Key]
        public int ImageID { get; set; }
        public string ImagePath { get; set; } = null!;
        public bool ImageStatus { get; set; }
        public int IntersectionID { get; set; }
        public Intersection intersection { get; set; } = null!;
    }
}
