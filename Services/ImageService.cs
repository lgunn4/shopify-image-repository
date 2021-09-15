using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using shopify_image_repository.Models;
using shopify_image_repository.Repository;

namespace shopify_image_repository.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
         private readonly IUserRepository _userRepository;
         public ImageService(IImageRepository imageRepository, IUserRepository userRepository)
         {
             _imageRepository = imageRepository;
             _userRepository = userRepository;
         }
         
        public ActionResult<IEnumerable<Image>> GetUserImages(string userName)
        {
            if (!UserNameExistsInDatabase(userName))
            {
                return new NotFoundResult();
            }

            var user = _userRepository.getUserByUserName(userName);
            return new ActionResult<IEnumerable<Image>>(_imageRepository.GetPrivateUserImages(user));
        }

        public ActionResult<IEnumerable<Image>> GetPublicImages()
        {
            return new ActionResult<IEnumerable<Image>>(_imageRepository.GetPublicImages());
        }

        public ActionResult<IEnumerable<Image>> GetPublicUserImages(string userId)
        {
            if (!UserIdExistsInDatabase(userId))
            {
                return new NotFoundResult();
            }

            var user = _userRepository.getUserById(userId);
            return new ActionResult<IEnumerable<Image>>(_imageRepository.GetPublicUserImages(user));
        }

        public ActionResult CreateImages(string userName)
        {
            throw new NotImplementedException();
        }

        public ActionResult DeleteUserImages(string userName, List<string> imageIds)
        {
            if (!UserNameExistsInDatabase(userName))
            {
                return new NotFoundResult();
            }

            var user = _userRepository.getUserByUserName(userName);
            _imageRepository.removeImages(user, imageIds);
            return new OkResult();
        }

        public ActionResult DeleteAllImages(string userName)
        {
            if (!UserNameExistsInDatabase(userName))
            {
                return new NotFoundResult();
            }
            
            var user = _userRepository.getUserByUserName(userName);
            _imageRepository.removeAllImages(user);
            return new OkResult();
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
}
