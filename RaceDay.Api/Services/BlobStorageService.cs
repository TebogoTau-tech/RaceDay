using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using RaceDay.Api.Services;

namespace RaceDay.Api.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureBlobStorage")
                ?? throw new InvalidOperationException("Azure Storage connection string missing in configuration.");

            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is null or empty.", nameof(file));

            // Get a reference to the container and create it if it doesn't exist
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            // Generate a unique filename to prevent naming collisions
            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var blobClient = containerClient.GetBlobClient(uniqueFileName);

            // Upload the file stream
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

            // Return the public URL of the uploaded blob
            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteFileAsync(string fileUrl, string containerName)
        {
            if (string.IsNullOrEmpty(fileUrl)) return false;

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            string fileName = Path.GetFileName(new Uri(fileUrl).AbsolutePath);
            var blobClient = containerClient.GetBlobClient(fileName);

            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
