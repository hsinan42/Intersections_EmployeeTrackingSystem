using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class PendingChangeDetailVM
    {
        public int RequestId { get; set; }
        public ChangeType ChangeType { get; set; }
        public int? IntersectionID { get; set; }
        public string IntersectionTitle { get; set; } = "-";
        public string RequestedByUserName { get; set; } = "-";
        public DateTime RequestedAt { get; set; }

        public List<FieldDiff> ScalarDiffs { get; set; } = new();
        public List<FieldDiff> SubstructureDiffs { get; set; } = new();
        public List<LocationChange> LocationDiffs { get; set; } = new();
        public List<ImageChange> ImageDiffs { get; set; } = new();
    }
    public class FieldDiff
    {
        public string Path { get; set; } = "";
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string Label { get; set; } = "";
    }

    public class LocationChange
    {
        public string ChangeType { get; set; } = "";
        public int? LocationID { get; set; }
        public List<FieldDiff> FieldDiffs { get; set; } = new();
    }

    public class ImageChange
    {
        public string ChangeType { get; set; } = "";
        public int? ImageId { get; set; }
        public string? Path { get; set; }
    }
}
