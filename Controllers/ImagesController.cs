using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shopify_image_repository.Models;
using shopify_image_repository.Services;

namespace shopify_image_repository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }
        
        [Authorize]
        [HttpGet("user")]
        public ActionResult<IEnumerable<Image>> GetUserImages()
        {
            return _imageService.GetUserImages(User.Identity.Name);
        }
        
        [HttpGet("public")]
        public ActionResult<IEnumerable<Image>> GetPublicImages([FromQuery] string userId = null)
        {
            return userId is null ? _imageService.GetPublicImages() : _imageService.GetPublicUserImages(userId);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<string> CreateImage()
        {
            return _imageService.CreateImages(User.Identity.Name);
        }
        
        [Authorize]
        [HttpDelete]
        public ActionResult DeleteImages([FromBody] List<string> imageIds)
        {
            return _imageService.DeleteUserImages(User.Identity.Name, imageIds);
        }
        
        [Authorize]
        [HttpDelete("all")]
        public ActionResult<string> DeleteAllImages()
        {
            return _imageService.DeleteAllImages(User.Identity.Name);
        }
    }
}