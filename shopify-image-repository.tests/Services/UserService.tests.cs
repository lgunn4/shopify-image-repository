using System;
using Moq;
using shopify_image_repository.Models;
using shopify_image_repository.Repository;
using shopify_image_repository.Services;
using Xunit;

namespace shopify_image_repository.tests.Services
{
    public class UserService_tests
    {

        [Fact]
        public void GetUserByUserName_UserNotExists_CreatesUser()
        {
            var mockedUserRepository = new Mock<IUserRepository>();
            mockedUserRepository.Setup(mock => mock.getUserByUserName(It.IsAny<string>())).Returns((User) null);

            var service = new UserService(mockedUserRepository.Object);
            service.GetUserByUserName("Logan");
            
            mockedUserRepository.Verify(mock => mock.addUser(It.IsAny<User>()), Times.Once);
        }
        
        [Fact]
        public void GetUserByUserName_UserExists_ReturnsUser()
        {
            var expectedUser = new User {UserId = 1, UserName = "Logan"};
            var mockedUserRepository = new Mock<IUserRepository>();
            mockedUserRepository.Setup(mock => mock.getUserByUserName(It.IsAny<string>())).Returns(expectedUser);
            
            
            var service = new UserService(mockedUserRepository.Object);
            var userResult = service.GetUserByUserName(expectedUser.UserName);
            
            Assert.Equal(expectedUser, userResult);
        }

        [Fact]
        public void GetUserByUserId_UserNotExists_ThrowsUserNotFound()
        {
            var mockedUserRepository = new Mock<IUserRepository>();
            mockedUserRepository.Setup(mock => mock.getUserById(It.IsAny<string>())).Returns((User) null);

            var service = new UserService(mockedUserRepository.Object);
            Action act = () => service.GetUserByUserId("Logan");

            Assert.Throws<UserNotFoundException>(act);
        }
        
        [Fact]
        public void GetUserByUserId_UserExists_ReturnsUser()
        {
            var expectedUser = new User {UserId = 1, UserName = "Logan"};
            var mockedUserRepository = new Mock<IUserRepository>();
            mockedUserRepository.Setup(mock => mock.getUserById(It.IsAny<string>())).Returns(expectedUser);

            var service = new UserService(mockedUserRepository.Object);
            var userResult = service.GetUserByUserId(expectedUser.UserId.ToString());
            
            Assert.Equal(expectedUser, userResult);
        }
        
    }
}