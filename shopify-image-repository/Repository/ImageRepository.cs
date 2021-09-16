using System.Collections.Generic;
using System.Linq;
using shopify_image_repository.Data;
using shopify_image_repository.Models;

namespace shopify_image_repository.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly UserImageDbContext _userImageDbContext;
        public ImageRepository(UserImageDbContext userImageDbContext)
        {
            _userImageDbContext = userImageDbContext;
        }
        
        public IEnumerable<Image> GetUserImages(User user)
        {
            return _userImageDbContext.Images
                .Where(image => image.UserId == user.UserId)
                .AsEnumerable();
        }

        public IEnumerable<Image> GetUserImagesByIds(User user, List<int> imageIds)
        {
            return _userImageDbContext.Images
                .Where(image => image.UserId == user.UserId && imageIds.Any(imageId => image.ImageId == imageId))
                .AsEnumerable();
        }

        public IEnumerable<Image> GetPrivateUserImages(User user)
        {
            return _userImageDbContext.Images
                .Where(image => !image.IsPublic && image.UserId == user.UserId)
                .AsEnumerable();
        }
        public IEnumerable<Image> GetPublicUserImages(User user)
        {
            return _userImageDbContext.Images
                .Where(image => image.IsPublic && image.UserId == user.UserId)
                .AsEnumerable();
        }

        public IEnumerable<Image> GetPublicImages()
        {
            return _userImageDbContext.Images
                .Where(image => image.IsPublic)
                .AsEnumerable();
        }

        public void AddImage(Image image)
        {
            _userImageDbContext.Add(image);
            _userImageDbContext.SaveChanges();
        }

        public void RemoveImages(List<Image> images)
        {
            foreach (var image in images)
            {  
                _userImageDbContext.Remove(image);
            }
            _userImageDbContext.SaveChanges();
        }
    }
}