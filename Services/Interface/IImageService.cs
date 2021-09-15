using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopify_image_repository.Models;

namespace shopify_image_repository.Services
{
    public interface IImageService
    {
        public ActionResult<IEnumerable<Image>> GetUserImages(string userName);
        public ActionResult<IEnumerable<Image>> GetPublicImages();
        public ActionResult<IEnumerable<Image>> GetPublicUserImages(string userId);
        public ActionResult CreateImages(string userName);
        public ActionResult DeleteUserImages(string userName, List<string> imageIds);
        public ActionResult DeleteAllImages(string userName);
    }
}