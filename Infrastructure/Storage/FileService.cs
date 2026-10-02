using Application.Common.Interfaces;
using Infrastructure.Storage.Documents;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Storage
{
    public class FileService : IFileService
    {
        public string UploadFile(IFormFile file, string folderName)
        {
            return DocumentSetting.Upload(file, folderName);
        }

        public void DeleteFile(string fileName, string folderName)
        {
            DocumentSetting.Delete(fileName, folderName);
        }
    }
}
