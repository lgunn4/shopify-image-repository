using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using shopify_image_repository.Models;

namespace shopify_image_repository.Services
{
    public interface IBlobStorageManager
    {
        Task upload(IFormFile imageFile, string blobId);
        Task delete(List<Image> images);
        string getImageUrl(string blobId);
    }
}