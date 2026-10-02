using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface IFileService
    {
        string UploadFile(IFormFile file, string folderName);
        void DeleteFile(string fileName, string folderName);
    }
}
