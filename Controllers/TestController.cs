using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopify_image_repository.Data;

namespace shopify_image_repository.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController
    {
        private readonly ImageRepositoryContext _context;
        public TestController(ImageRepositoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<string> Test([FromQuery] int userId)
        {
            var test = _context.Users.FirstOrDefault(user => user.UserId.Equals(userId));
            return test.UserName.ToString();
        }
    }
}