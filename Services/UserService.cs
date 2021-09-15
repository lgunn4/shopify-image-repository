using System;
using Microsoft.AspNetCore.Mvc;
using shopify_image_repository.Models;
using shopify_image_repository.Repository;

namespace shopify_image_repository.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUserByUserName(string userName)
        {
            if (!UserNameExistsInDatabase(userName))
            {
                _userRepository.addUser(new User {UserName = userName});
            }
            return _userRepository.getUserByUserName(userName);
        }

        public User GetUserByUserId(string userId)
        {
            if (!UserIdExistsInDatabase(userId))
            {
                throw new UserNotFoundException();
            }

            return _userRepository.getUserById(userId);
        }
        
        private bool UserNameExistsInDatabase(string userName)
        {
            var user = _userRepository.getUserByUserName(userName);
            return user != null;
        }

        private bool UserIdExistsInDatabase(string userId)
        {
            var user = _userRepository.getUserById(userId);
            return user != null;
        }
    }

    public class UserNotFoundException : Exception
    {
    }
}