using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.IServices
{
    public interface IFileService
    {
        Task<string?> SaveFileAsync(string? oldFilePath, IFormFile? newFile);
        void DeleteFile(string? filePath);
    }
}
