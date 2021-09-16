using System;
using System.Linq;
using shopify_image_repository.Data;
using shopify_image_repository.Models;

namespace shopify_image_repository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserImageDbContext _userImageDbContext;

        public UserRepository(UserImageDbContext userImageDbContext)
        {
            _userImageDbContext = userImageDbContext;
        }
        
        public User getUserByUserName(string userName)
        {
            return _userImageDbContext.Users.FirstOrDefault(user => user.UserName == userName);
        }

        public User getUserById(string id)
        {
            return _userImageDbContext.Users.FirstOrDefault(user => user.UserId == Convert.ToInt32(id));
        }

        public void addUser(User user)
        {
            _userImageDbContext.Add(user);
            _userImageDbContext.SaveChanges();
        }
    }
}