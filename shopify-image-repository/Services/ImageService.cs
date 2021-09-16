using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shopify_image_repository.Models;
using shopify_image_repository.Repository;

namespace shopify_image_repository.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IUserService _userService;
        private readonly IBlobStorageManager _blobStorageManager;
         public ImageService(IImageRepository imageRepository, IUserService userService, IBlobStorageManager blobStorageManager)
         {
             _imageRepository = imageRepository;
             _userService = userService;
             _blobStorageManager = blobStorageManager;
         }
         
        public ActionResult<IEnumerable<Image>> GetUserImages(string userName)
        {
            var user = _userService.GetUserByUserName(userName);
            return new ActionResult<IEnumerable<Image>>(_imageRepository.GetUserImages(user));
        }

        public ActionResult<IEnumerable<Image>> GetPrivateUserImages(string userName)
        {
            var user = _userService.GetUserByUserName(userName);
            return new ActionResult<IEnumerable<Image>>(_imageRepository.GetPrivateUserImages(user));
        }
        
        public ActionResult<IEnumerable<Image>> GetPublicImages()
        {
            return new ActionResult<IEnumerable<Image>>(_imageRepository.GetPublicImages());
        }

        public ActionResult<IEnumerable<Image>> GetPublicUserImages(string userId)
        {
            try
            {
                var user = _userService.GetUserByUserId(userId);
                return new ActionResult<IEnumerable<Image>>(_imageRepository.GetPublicUserImages(user));
            } 
            catch (UserNotFoundException)
            {
                return new NotFoundResult();
            }
        }

        public async Task<ActionResult> CreateImages(string userName, List<IFormFile> imageFiles, ImageMetadataModel imageMetadataModel)
        {
            var user = _userService.GetUserByUserName(userName);
            foreach (var imageFile in imageFiles)
            {
                var image = BuildImage(imageMetadataModel, user.UserId);
                await _blobStorageManager.upload(imageFile, image.blobId);
                _imageRepository.AddImage(image);
            }
            return new OkResult();
        }

        public ActionResult DeleteUserImages(string userName, List<int> imageIds)
        {
            var user = _userService.GetUserByUserName(userName);
            var imagesToDelete = _imageRepository.GetUserImagesByIds(user, imageIds).ToList();
            
            _imageRepository.RemoveImages(imagesToDelete);
            _blobStorageManager.delete(imagesToDelete);
            return new OkResult();
        }

        public ActionResult DeleteAllImages(string userName)
        {
            var user = _userService.GetUserByUserName(userName);
            var imagesToDelete = _imageRepository.GetUserImages(user).ToList();
            
            _imageRepository.RemoveImages(imagesToDelete);
            _blobStorageManager.delete(imagesToDelete);
            return new OkResult();
        }

        private Image BuildImage(ImageMetadataModel imageMetadataModel, int userId)
        {
            var blobId = Guid.NewGuid().ToString();
            return new Image
            {
                ImageDescription = imageMetadataModel.Description,
                Location = imageMetadataModel.Location,
                blobId = blobId,
                ImageUrl = _blobStorageManager.getImageUrl(blobId),
                UploadDate = DateTime.Now,
                UserId = userId,
                IsPublic = imageMetadataModel.IsPublic
            };
        }
    }
}
