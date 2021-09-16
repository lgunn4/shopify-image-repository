using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using shopify_image_repository.Models;

namespace shopify_image_repository.Services
{
    public class AzureBlobStorageManager : IBlobStorageManager
    {
        private readonly IConfiguration _configuration;
        private readonly BlobContainerClient _blobContainerClient;
        public AzureBlobStorageManager(IConfiguration configuration, BlobServiceClient blobServiceClient)
        {
            _configuration = configuration;
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(_configuration.GetConnectionString("AzureContainerName"));
        }
        
        public async Task upload(IFormFile imageFile, string blobId)
        {
            var blobClient = _blobContainerClient.GetBlobClient(blobId);
            await blobClient.UploadAsync(imageFile.OpenReadStream());
        }

        public async Task delete(List<Image> images)
        {
            List<Task> deleteTasks = new List<Task>();
            foreach (var image in images)
            {
                var blobClient = _blobContainerClient.GetBlobClient(image.blobId);
                deleteTasks.Add(blobClient.DeleteAsync());
            }

            await Task.WhenAll(deleteTasks);
        }

        public string getImageUrl(string blobId)
        {
            return _configuration.GetConnectionString("AzureImageUrlPrefix") + blobId;
        }
    }
}