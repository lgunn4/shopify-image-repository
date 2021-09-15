using shopify_image_repository.Models;

namespace shopify_image_repository.Services
{
    public interface IUserService
    {
        public User GetUserByUserName(string userName);
        public User GetUserByUserId(string userId);
    }
}