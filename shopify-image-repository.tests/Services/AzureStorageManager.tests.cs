using System.Collections.Generic;
using System.IO;
using System.Text;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using shopify_image_repository.Models;
using shopify_image_repository.Services;
using Xunit;

namespace shopify_image_repository.tests.Services
{
    public class AzureStorageManager_tests
    {
        private readonly Mock<BlobServiceClient> _mockedBlobServiceClient;
        private readonly Mock<BlobContainerClient> _mockedBlobContainerClient;
        private readonly Mock<BlobClient> _mockedBlobClient;
        private readonly IConfiguration _testConfiguration;
        private const string ContainerId = "test-container-name";

        public AzureStorageManager_tests()
        {
            _mockedBlobServiceClient = new Mock<BlobServiceClient>();
            _mockedBlobContainerClient = new Mock<BlobContainerClient>();
            _mockedBlobClient = new Mock<BlobClient>();
            _testConfiguration = InitConfiguration();

            _mockedBlobContainerClient.Setup(mock => mock.GetBlobClient(It.IsAny<string>())).Returns(_mockedBlobClient.Object);
            _mockedBlobServiceClient.Setup(mock => mock.GetBlobContainerClient(ContainerId))
                .Returns(_mockedBlobContainerClient.Object);
        }

        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }
        
        [Fact]
        public async void Upload_CallsBlobClient()
        {
            const string blobId = "akjbfadg9ag907adf0a";
            var service = new AzureBlobStorageManager(_testConfiguration, _mockedBlobServiceClient.Object);

            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            await service.upload(file, blobId);

            _mockedBlobClient.Verify(mock => mock.UploadAsync(It.IsAny<Stream>()));
        }
        
        [Fact]
        public async void Delete_CreatesBlobClientForEachImage()
        {
            var service = new AzureBlobStorageManager(_testConfiguration, _mockedBlobServiceClient.Object);

            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            
            var testImages = Generate_Test_Images();
            await service.delete(testImages);

            _mockedBlobContainerClient.Verify(mock => mock.GetBlobClient(It.IsAny<string>()), Times.Exactly(testImages.Count));
        }
        
        [Fact]
        public async void getImageUrl_AppendsUrlConfiguration()
        {
            var service = new AzureBlobStorageManager(_testConfiguration, _mockedBlobServiceClient.Object);
            const string testBlobId = "testBlobId";
            var expectedResult = _testConfiguration.GetConnectionString("AzureImageUrlPrefix") + testBlobId;
            
            var result = service.getImageUrl("testBlobId");

            Assert.Equal(expectedResult, result);
        }
        
        private List<Image> Generate_Test_Images()
        {
            return new List<Image>
            {
                new Image {blobId = "testBlob1"},
                new Image {blobId = "testBlob2"},
                new Image {blobId = "testBlob3"},
                new Image {blobId = "testBlob4"},
            };
        }
    }
}