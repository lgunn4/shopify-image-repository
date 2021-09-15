using shopify_image_repository.Models;

namespace shopify_image_repository.Repository
{
    public interface IUserRepository
    {
        User getUserByUserName(string userName);
        User getUserById(string id);
        void addUser(User user);
    }
}