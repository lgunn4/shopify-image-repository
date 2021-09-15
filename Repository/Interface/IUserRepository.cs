using shopify_image_repository.Models;

namespace shopify_image_repository.Repository
{
    public interface IUserRepository
    {
        User getUserByUserName(string userName);
        void addUser(User user);
    }
}