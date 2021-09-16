using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using shopify_image_repository.Models;
using shopify_image_repository.Repository;
using shopify_image_repository.Services;
using Xunit;

namespace shopify_image_repository.tests.Services
{
    public class ImageService_tests
    {
        private readonly Mock<IBlobStorageManager> _mockedBlobService;
        private readonly Mock<IUserService> _mockedUserService;
        private readonly Mock<IImageRepository> _mockedImageRepository;
        private readonly User _testUser;
        private readonly IEnumerable<Image> _testImages;
        
        public ImageService_tests()
        {
            _mockedBlobService = new Mock<IBlobStorageManager>();       
            _mockedUserService = new Mock<IUserService>();              
            _mockedImageRepository = new Mock<IImageRepository>();  
            
            _testUser = new User
            {
                UserId = 1, 
                UserName = "DaVinci"
            };
            _testImages = new List<Image>
            {
                new Image
                {
                    ImageDescription = "Mona Lisa", 
                    Location = "Florence", 
                    UserId = _testUser.UserId
                },
                new Image
                {
                    ImageDescription = "The Last Supper", 
                    Location = "Florence", 
                    UserId = _testUser.UserId
                },
            }.AsEnumerable();
        }
        
        [Fact]
        public void GetUserImages_CallsUserService_ReturnsRepositoryUserImages()
        {
            _mockedUserService.Setup(mock => mock.GetUserByUserName(It.IsAny<string>())).Returns(_testUser);
            _mockedImageRepository.Setup(mock => mock.GetUserImages(It.IsAny<User>())).Returns(_testImages);
            var service = new ImageService(
                _mockedImageRepository.Object, 
                _mockedUserService.Object,
                _mockedBlobService.Object);

            var resultImages = service.GetUserImages(_testUser.UserName);
            
            _mockedUserService.Verify(mock => mock.GetUserByUserName(It.IsAny<string>()), Times.Once);
            Assert.Equal(_testImages, resultImages.Value);
        }
        
        [Fact]
        public void GetPrivateUserImages_CallsUserService_ReturnsRepositoryPrivateUserImages()
        {
            _mockedUserService.Setup(mock => mock.GetUserByUserName(It.IsAny<string>())).Returns(_testUser);
            _mockedImageRepository.Setup(mock => mock.GetPrivateUserImages(It.IsAny<User>())).Returns(_testImages);
            var service = new ImageService(
                _mockedImageRepository.Object, 
                _mockedUserService.Object,
                _mockedBlobService.Object);

            var resultImages = service.GetPrivateUserImages(_testUser.UserName);
            
            _mockedUserService.Verify(mock => mock.GetUserByUserName(It.IsAny<string>()), Times.Once);
            Assert.Equal(_testImages, resultImages.Value);
        }
        
        [Fact]
        public void GetPublicImages_ReturnsRepositoryPublicImages()
        {
            _mockedImageRepository.Setup(mock => mock.GetPublicImages()).Returns(_testImages);
            var service = new ImageService(
                _mockedImageRepository.Object, 
                _mockedUserService.Object,
                _mockedBlobService.Object);

            var resultImages = service.GetPublicImages();
            
            Assert.Equal(_testImages, resultImages.Value);
        }
        
        [Fact]
        public void GetPublicUserImages_UserNotExist_ReturnsNotFoundResult()
        {
            _mockedUserService.Setup(mock => mock.GetUserByUserId(It.IsAny<string>())).Throws<UserNotFoundException>();
            var service = new ImageService(
                _mockedImageRepository.Object, 
                _mockedUserService.Object,
                _mockedBlobService.Object);

            var result = service.GetPublicUserImages(_testUser.UserId.ToString());
            
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public void GetPublicUserImages_UserExists_ReturnRepositoryPublicUserImages()
        {
            _mockedUserService.Setup(mock => mock.GetUserByUserId(It.IsAny<string>())).Returns(_testUser);
            _mockedImageRepository.Setup(mock => mock.GetPublicUserImages(It.IsAny<User>())).Returns(_testImages);
            var service = new ImageService(
                _mockedImageRepository.Object, 
                _mockedUserService.Object,
                _mockedBlobService.Object);

            var resultImages = service.GetPublicUserImages(_testUser.UserId.ToString());
            
            _mockedUserService.Verify(mock => mock.GetUserByUserId(It.IsAny<string>()), Times.Once);
            Assert.Equal(_testImages, resultImages.Value);
        }

        [Fact]
        public void CreateImages_WithImageFiles_CallsBlobServiceAndImageRepositoryForEachImage()
        {
            var imageMetadata = new ImageMetadataModel
            {
                IsPublic = true,
                Description = "recently uploaded", 
                Location = "Kingston, On"
            };

            var imageFiles = new List<IFormFile>
            {
                new Mock<IFormFile>().Object,
                new Mock<IFormFile>().Object,
                new Mock<IFormFile>().Object,
                new Mock<IFormFile>().Object
            };
            
            _mockedUserService.Setup(mock => mock.GetUserByUserName(It.IsAny<string>())).Returns(_testUser);
            
            var service = new ImageService(
                _mockedImageRepository.Object, 
                _mockedUserService.Object,
                _mockedBlobService.Object);
            
            service.CreateImages(_testUser.UserName, imageFiles, imageMetadata);

            _mockedBlobService.Verify(mock => mock.upload(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Exactly(imageFiles.Count));
            _mockedImageRepository.Verify(mock => mock.addImage(It.IsAny<Image>()), Times.Exactly(imageFiles.Count));
        }
        
        [Fact]
        public void CreateImages_WithNoImageFiles_DoesNotCallBlobServiceAndImageRepository()
        {
            var imageMetadata = new ImageMetadataModel
            {
                IsPublic = true,
                Description = "recently uploaded", 
                Location = "Kingston, On"
            };

            var imageFiles = new List<IFormFile>();

            _mockedUserService.Setup(mock => mock.GetUserByUserName(It.IsAny<string>())).Returns(_testUser);
            
            var service = new ImageService(
                _mockedImageRepository.Object, 
                _mockedUserService.Object,
                _mockedBlobService.Object);
            
            service.CreateImages(_testUser.UserName, imageFiles, imageMetadata);

            _mockedBlobService.Verify(mock => mock.upload(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Exactly(imageFiles.Count));
            _mockedImageRepository.Verify(mock => mock.addImage(It.IsAny<Image>()), Times.Exactly(imageFiles.Count));
        }
        
        [Fact]
        public void DeleteUserImages_CallsRepositoryAndBlobService()
        {
            var imageIds = new List<int> {1, 2, 3, 77};
            _mockedUserService.Setup(mock => mock.GetUserByUserName(It.IsAny<string>())).Returns(_testUser);
            _mockedImageRepository.Setup(mock => mock.GetUserImagesByIds(It.IsAny<User>(), It.IsAny<List<int>>())).Returns(_testImages);
            var service = new ImageService(
                _mockedImageRepository.Object, 
                _mockedUserService.Object,
                _mockedBlobService.Object);

            service.DeleteUserImages(_testUser.UserName, imageIds);
            
            _mockedUserService.Verify(mock => mock.GetUserByUserName(It.IsAny<string>()), Times.Once);
            _mockedBlobService.Verify(mock => mock.delete(It.IsAny<List<Image>>()), Times.Once);
            _mockedImageRepository.Verify(mock => mock.removeImages(It.IsAny<List<Image>>()), Times.Once);
        }
        
        [Fact]
        public void DeleteAllImages_CallsRepositoryAndBlobService()
        {
            var imageIds = new List<int> {1, 2, 3, 77};
            _mockedUserService.Setup(mock => mock.GetUserByUserName(It.IsAny<string>())).Returns(_testUser);
            _mockedImageRepository.Setup(mock => mock.GetUserImages(It.IsAny<User>())).Returns(_testImages);
            var service = new ImageService(
                _mockedImageRepository.Object, 
                _mockedUserService.Object,
                _mockedBlobService.Object);

            service.DeleteAllImages(_testUser.UserName);
            
            _mockedUserService.Verify(mock => mock.GetUserByUserName(It.IsAny<string>()), Times.Once);
            _mockedBlobService.Verify(mock => mock.delete(It.IsAny<List<Image>>()), Times.Once);
            _mockedImageRepository.Verify(mock => mock.removeImages(It.IsAny<List<Image>>()), Times.Once);
        }
    }
}