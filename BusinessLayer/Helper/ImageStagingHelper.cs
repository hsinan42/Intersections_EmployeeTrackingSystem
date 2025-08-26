namespace BusinessLayer.Helper
{
    public class ImageStagingHelper
    {
        public static string MoveFromStaging(string stagingRelPath)
        {
            var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var src = Path.Combine(webRoot, stagingRelPath.TrimStart('/'));
            var fileName = Path.GetFileName(src);
            var dstRel = "/uploads/Images/" + fileName;
            var dst = Path.Combine(webRoot, "uploads/Images", fileName);

            if (File.Exists(src))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dst)!);
                File.Move(src, dst, overwrite: true);
            }
            return dstRel;
        }

    }
}
