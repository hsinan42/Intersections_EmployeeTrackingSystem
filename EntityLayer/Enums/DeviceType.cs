using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Enums
{
    public enum DeviceType
    {
        [Display(Name = "Henüz Seçilmedi")]
        Unknown = 0,

        [Display(Name = "3U")]
        U3,
        [Display(Name = "3U looplu")]
        U3withLoop,
        [Display(Name = "3U yaya basgeç")]
        U3PedPress,

        [Display(Name = "6U")]
        U6,
        [Display(Name = "6U looplu")]
        U6withLoop,
        [Display(Name = "6U adaptif")]
        U6Adaptive,
        [Display(Name = "6U hibrit")]
        U6Hybrid,
        [Display(Name = "6U yaya basgeç")]
        U6PedPress,

        [Display(Name = "Y3U")]
        YU3,
        [Display(Name = "Y3U looplu")]
        YU3withLoop,
        [Display(Name = "Y3U adaptif")]
        YU3Adaptive,
        [Display(Name = "Y3U hibrit")]
        YU3Hybrid,
        [Display(Name = "Y3U yaya basgeç")]
        YU3PedPress
    }
}
