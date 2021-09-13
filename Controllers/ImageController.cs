using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace shopify_image_repository.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[]
            {
                User.Identity.Name
            };
        }
    }
}