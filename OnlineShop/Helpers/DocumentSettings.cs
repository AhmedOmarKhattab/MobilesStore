using System.IO;

namespace OnlineShop.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadImage(IFormFile file, string FolderName)
        {
            // 1.Get located folder path
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);
            // 2. Get file name and make it unique
            string FileName = $"{Guid.NewGuid()}{file.FileName}";
            // 3. Get file path
            string FilePath = Path.Combine(FolderPath, FileName);

            var fs = new FileStream(FilePath, FileMode.CreateNew);

            file.CopyTo(fs);

            return FileName;
        }

            
    }
}
