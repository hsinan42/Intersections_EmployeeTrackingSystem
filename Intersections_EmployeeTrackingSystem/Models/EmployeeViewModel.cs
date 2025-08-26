namespace Intersections_EmployeeTrackingSystem.Models
{
    public class EmployeeViewModel
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<EmployeeIntersectionsVM> EmployeIntersections { get; set; } = new();
        public List<EmployeeReportsVM> EmployeeReports { get; set; } = new();

    }

    public class EmployeeIntersectionsVM
    {
        public int IntersectionID { get; set; }
        public int KkcID { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IntersectionStatus { get; set; }
    }

    public class EmployeeReportsVM
    {
        public int ReportID { get; set; }
        public DateTime ReportDate { get; set; }
        public string? ReportName { get; set; }
        public string? ReportDescription { get; set; }
        public int IntersectionID { get; set; }
        public string? IntersectionTitle { get; set; }
        public bool IntersectionStatus { get; set; }
        public int KkcId { get; set; }
        public int? UserID { get; set; }
    }
}
