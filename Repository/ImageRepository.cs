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

        public void addImage(Image image)
        {
            _userImageDbContext.Add(image);
            _userImageDbContext.SaveChanges();
        }

        public void removeImages(User user, List<string> imageIds)
        {
            var imagesToRemove = new List<Image>();
            foreach (var imageId in imageIds)
            {
                var image = _userImageDbContext.Images.FirstOrDefault(dbImage => dbImage.UserId == user.UserId && dbImage.ImageId.ToString() == imageId);
                if (image != null)
                {
                    imagesToRemove.Add(image);
                }
            }
            _userImageDbContext.Remove(imagesToRemove);
            _userImageDbContext.SaveChanges();
        }

        public void removeAllImages(User user)
        {
            var imagesToRemove = _userImageDbContext.Images.Where(image => image.UserId == user.UserId).GetEnumerator();
            _userImageDbContext.Remove(imagesToRemove);
            _userImageDbContext.SaveChanges();
        }
    }
}