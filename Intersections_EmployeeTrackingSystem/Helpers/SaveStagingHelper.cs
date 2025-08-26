namespace Intersections_EmployeeTrackingSystem.Helpers
{
    public class SaveStagingHelper
    {
        public static string SaveToStaging(IFormFile file)
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "Images", "staging");
            Directory.CreateDirectory(root);
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var abs = Path.Combine(root, fileName);
            using var fs = new FileStream(abs, FileMode.Create);
            file.CopyTo(fs);
            return "/uploads/Images/staging/" + fileName;
        }
    }
}
