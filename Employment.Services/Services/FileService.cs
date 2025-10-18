using Job.Services.JobServices.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Services
{
    public class FileService : IFileService
    {
        private readonly string _basePath = "C:\\ImageForJobProject\\";

        public async Task<string?> SaveFileAsync(string? oldFilePath, IFormFile? newFile)
        {
            DeleteFile(oldFilePath);

            if (newFile == null || newFile.Length == 0)
                return null;

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(newFile.FileName)}";
            var path = Path.Combine(_basePath, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await newFile.CopyToAsync(stream);

            return path;
        }

        public void DeleteFile(string? filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
