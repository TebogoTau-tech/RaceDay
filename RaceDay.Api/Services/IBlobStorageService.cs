namespace RaceDay.Api.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string containerName);
        Task<bool> DeleteFileAsync(string fileUrl, string containerName);
    }
}
