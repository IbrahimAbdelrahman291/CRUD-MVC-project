using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace DemoPL.Helpers
{
    public static class DocumentSettings
    {
        //upload file
        public static string Upload(IFormFile file,string FolderName) 
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files",FolderName);

            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            string FilePath = Path.Combine(FolderPath,FileName);
            using var fs = new FileStream(FilePath,FileMode.Create);
            file.CopyTo(fs);
            return FileName;
        }
        //delete file
        public static void Delete(string FileName,string FolderName)
        {
            //1- get file path
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files",FolderName,FileName);
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            else
            {
                return;
            }
        }
    }
}
