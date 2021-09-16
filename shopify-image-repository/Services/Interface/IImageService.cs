using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shopify_image_repository.Models;

namespace shopify_image_repository.Services
{
    public interface IImageService
    {
        public ActionResult<IEnumerable<Image>> GetUserImages(string userName);
        public ActionResult<IEnumerable<Image>> GetPrivateUserImages(string userName);
        public ActionResult<IEnumerable<Image>> GetPublicImages();
        public ActionResult<IEnumerable<Image>> GetPublicUserImages(string userId);
        public Task<ActionResult> CreateImages(string userName, List<IFormFile> imageFiles, ImageMetadataModel imageMetadataModel);
        public ActionResult DeleteUserImages(string userName, List<int> imageIds);
        public ActionResult DeleteAllImages(string userName);
    }
}