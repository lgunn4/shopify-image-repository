using System.Collections;
using System.Collections.Generic;
using shopify_image_repository.Data;
using shopify_image_repository.Models;

namespace shopify_image_repository.Repository
{
    public interface IImageRepository
    {
        IEnumerable<Image> GetPrivateUserImages(User user);
        IEnumerable<Image> GetPublicUserImages(User user);
        IEnumerable<Image> GetPublicImages();
        void addImage(Image image);
        void removeImages(User user, List<string> imageIds);
        void removeAllImages(User user);
    }
}