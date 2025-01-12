using System.Net;
using AvaStorage.Application.UseCases.GetAvatar;
using AvaStorage.Domain;
using Moq;
using MyLab.ApiClient.Test;
using MyLab.AvaStorage;

namespace AvaStorage.Tests
{
    public partial class AdminGetApiBehavior : IClassFixture<TestApiFixture<Program, IAvaStorageV1>>
    {
        [Fact]
        public async Task ShouldGetPicture()
        {
            //Arrange
            _getHandlerMock
                .Setup(
                    h => h.Handle
                    (
                        It.IsAny<GetAvatarCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                .ReturnsAsync(new GetAvatarResult(new MemoryAvatarFile(TestTools.PictureBin)));

            var client = CreateClient();

            //Act
            var response = await client.GetAsync("foo", 64, "admin");

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(TestTools.PictureBin, response.ResponseContent);
            VerifyHandlerCall("foo", 64, "admin");
        }

        [Fact]
        public async Task ShouldReturn404IfNotFound()
        {
            //Arrange
            _getHandlerMock
             .Setup(
                 h => h.Handle
                 (
                     It.IsAny<GetAvatarCommand>(),
                     It.IsAny<CancellationToken>()
                 ))
             .ReturnsAsync(new GetAvatarResult(null));

            var client = CreateClient();

            //Act
            var response = await client.GetAsync("foo", 64, "admin");

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturn400IfBadRequest()
        {
            //Arrange
            var client = CreateClient();

            //Act
            var response = await client.GetAsync("foo", -1, null);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task ShouldReturn304IfNotModified()
        {
            //Arrange
            var avaFile = new MemoryAvatarFile(TestTools.PictureBin)
            {
                LastModified = DateTimeOffset.Now.AddHours(-1)
            };

            _getHandlerMock
                .Setup(
                    h => h.Handle
                    (
                        It.IsAny<GetAvatarCommand>(),
                        It.IsAny<CancellationToken>()
                    ))
                .ReturnsAsync(new GetAvatarResult(avaFile));

            var client = CreateClient();

            //Act
            var response = await client.GetWithLastModifiedAsync("foo", 64, "admin", DateTimeOffset.Now.ToString("R"));

            //Assert
            Assert.Equal(HttpStatusCode.NotModified, response.StatusCode);
        }
    }
}