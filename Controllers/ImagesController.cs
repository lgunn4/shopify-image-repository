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
        public ActionResult<string> GetPublicImages([FromQuery] string userId = null)
        {
            if (userId is null)
            {
                return "Hello Public User! \nWelcome to the API";

            } else if (User.Identity.IsAuthenticated)
            {            
                return "Hello " + userId + "! \nWelcome to the API ";
            }
            return "You cannot specify userId if you arent authenticated";
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