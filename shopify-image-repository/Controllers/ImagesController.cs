using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        
        [Authorize]
        [HttpGet("private")]
        public ActionResult<IEnumerable<Image>> GetPrivateImages()
        {
            return _imageService.GetPrivateUserImages(User.Identity.Name);
        }
        
        [HttpGet("public")]
        public ActionResult<IEnumerable<Image>> GetPublicImages([FromQuery] string userId = null)
        {
            return userId is null ? _imageService.GetPublicImages() : _imageService.GetPublicUserImages(userId);
        }

        [Authorize]
        [HttpPost]
        public Task<ActionResult> CreateImages([FromForm] List<IFormFile> ImageFiles, [FromForm] ImageMetadataModel imageMetadataModel)
        {
            return _imageService.CreateImages(User.Identity.Name, ImageFiles, imageMetadataModel);
        }
        
        [Authorize]
        [HttpDelete]
        public ActionResult DeleteImages([FromBody] List<int> imageIds)
        {
            return _imageService.DeleteUserImages(User.Identity.Name, imageIds);
        }
        
        [Authorize]
        [HttpDelete("all")]
        public ActionResult DeleteAllImages()
        {
            return _imageService.DeleteAllImages(User.Identity.Name);
        }
    }
}