using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class IntersectionUpdateDto
    {
        public int IntersectionID { get; set; }
        [Display(Name ="Kavşak Adı")]
        public string Title { get; set; } = "";
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        public bool IntersectionStatus { get; set; } = true;
        [Display(Name = "Yol Adı")]
        public string RoadName { get; set; } = "";
        [Display(Name = "Bağlı Kurum")]
        public string BondedOrganisation { get; set; } = "";
        [Display(Name = "Kamera")]
        public int? CamCount { get; set; }
        [Display(Name = "Loop")]
        public int? LoopCount { get; set; }
        [Display(Name = "Grup")]
        public int? GroupCount { get; set; }
        [Display(Name = "Seri Numarası")]
        public string? SerialNumber { get; set; }
        [Display(Name = "Cpu Hex")]
        public string? CpuHex { get; set; }
        [Display(Name = "Driver Hex")]
        public string? DriverHex { get; set; }
        [Display(Name = "Cihaz Tipi")]
        public DeviceType DeviceType { get; set; }
        [Display(Name = "Yaya Butonu")]
        public bool PedButton { get; set; }
        [Display(Name = "UPS")]
        public bool UPS { get; set; }
        public SubstructureDto? Substructure { get; set; }
        public List<LocationDto> Locations { get; set; } = new();
        public ImageChangeDto Images { get; set; } = new();
    }

    public class SubstructureDto
    {
        public int? SubstructureID { get; set; }
        [Display(Name = "Altyapıyı Yapan")]
        public string? SubstructureBuilder { get; set; }
        [Display(Name = "Başlama Tarihi")]
        public DateTime? SubstructureStartDate { get; set; }
        [Display(Name = "Bitiş Tarihi")]
        public DateTime? SubstructureFinishDate { get; set; }
    }
    
    public class LocationDto
    {
        public int? LocationID { get; set; }
        [Display(Name = "Şehir")]
        public string? City { get; set; }
        [Display(Name = "İlçe")]
        public string? District { get; set; }
        [Display(Name = "Enlem")]
        public string? Latitude { get; set; }
        [Display(Name = "Boylam")]
        public string? Longitude { get; set; }
        [Display(Name = "Adres")]
        public string? Address { get; set; }
    }
    public class ImageChangeDto
    {
        public List<int> DeleteIds { get; set; } = new();
        public List<string> AddStagingPaths { get; set; } = new();
    }
}
