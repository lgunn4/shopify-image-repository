using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace shopify_image_repository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [Authorize]
        [HttpGet("user")]
        public ActionResult<string> GetUserImages()
        {
            return "Hello User: " + User.Identity.Name + "! \nWelcome to the API";
        }
        
        [HttpGet("public")]
        public ActionResult<string> GetPublicImages()
        {
            return "Hello Public User! \nWelcome to the API";
        }

        [Authorize]
        [HttpPost]
        public ActionResult<string> CreateImage()
        {
            return Ok();
        }
        
        [Authorize]
        [HttpDelete]
        public ActionResult<string> DeleteImages([FromBody] List<string> image_ids)
        {
            return Ok();
        }
        
        [Authorize]
        [HttpDelete("all")]
        public ActionResult<string> DeleteAllImages()
        {
            return Ok();
        }
    }
}