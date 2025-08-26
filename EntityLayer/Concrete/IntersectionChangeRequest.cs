using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class IntersectionChangeRequest
    {
        public int Id { get; set; }
        public int? IntersectionID { get; set; }
        public Intersection? Intersection { get; set; }

        public ChangeType ChangeType { get; set; }
        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;

        public string PayloadJson { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; } = DateTime.Now;

        public int RequestedByUserId { get; set; }
        public User RequestedBy { get; set; } = null!;

        public int? ReviewedByUserId { get; set; }
        public User? ReviewedBy { get; set; }

        public DateTime? ReviewedAt { get; set; }
        public string? ReviewNote { get; set; }

        public DateTime SnapshotUpdatedAt { get; set; }

    }
}
