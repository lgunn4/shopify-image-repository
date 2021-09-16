using System.Collections.Generic;
using shopify_image_repository.Models;

namespace shopify_image_repository.Repository
{
    public interface IImageRepository
    {
        IEnumerable<Image> GetUserImages(User user);
        IEnumerable<Image> GetUserImagesByIds(User user, List<int> imageIds);
        IEnumerable<Image> GetPrivateUserImages(User user);
        IEnumerable<Image> GetPublicUserImages(User user);
        IEnumerable<Image> GetPublicImages();
        void AddImage(Image image);
        void RemoveImages(List<Image> images);
    }
}