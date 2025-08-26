using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Substructure
    {
        [Key]
        public int SubstructureID { get; set; }
        public DateTime SubstructureStartDate { get; set; }
        public DateTime SubstructureFinishDate { get; set;}
        public string? SubstructureBuilder { get; set; }
        public int IntersectionID { get; set; }
        public Intersection intersection { get; set; } = null!;
    }
}
