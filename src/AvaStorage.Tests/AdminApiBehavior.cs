using System.Net;
using AvaStorage.Application.UseCases.PutAvatar;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using MyLab.ApiClient.Test;
using Xunit.Abstractions;

namespace AvaStorage.Tests
{
    public class AdminApiBehavior : IClassFixture<TestApiFixture<Program, IAdminContractV1>>
    {
        private readonly TestApiFixture<Program, IAdminContractV1> _fxt;

        public AdminApiBehavior(TestApiFixture<Program, IAdminContractV1> fxt, ITestOutputHelper output)
        {
            fxt.Output = output;
            _fxt = fxt;
        }
        
        [Fact]
        public async Task ShouldPutPicture()
        {
            //Arrange
            var putHandlerMock = new Mock<IRequestHandler<PutAvatarCommand>>();

            var outHandlerDescriptor = ServiceDescriptor.Transient(typeof(IRequestHandler<PutAvatarCommand>), s => putHandlerMock.Object);

            var proxyAsset = _fxt.StartWithProxy
                (
                    s => s.Replace(outHandlerDescriptor),
                    c =>
                    {
                        c.BaseAddress = new UriBuilder(c.BaseAddress!)
                        {
                            Port = ListenConstants.AdminPort 
                        }.Uri;
                    });
            var client = proxyAsset.ApiClient;

            var pictureBin = new byte[] { 1, 2, 3 };

            //Act
            var response = await client.PutAsync("foo", pictureBin);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            putHandlerMock.Verify(h => h.Handle
                (
                    It.Is<PutAvatarCommand>(c => 
                            c.Id == "foo" &&
                            c.Picture[0] == 1 &&
                            c.Picture[1] == 2 &&
                            c.Picture[2] == 3
                        ),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);
            putHandlerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(GetInvalidParameters))]
        public async Task ShouldReturn400WhenBadRequest(string id, byte[] picBin)
        {
            //Arrange
            var repoMock = new Mock<IPictureRepository>();

            var proxyAsset = _fxt.StartWithProxy
                (
                    s => s.AddSingleton(repoMock.Object),
                    c =>
                    {
                        c.BaseAddress = new UriBuilder(c.BaseAddress!)
                        {
                            Port = ListenConstants.AdminPort
                        }.Uri;
                    });
            var client = proxyAsset.ApiClient;

            //Act
            var response = await client.PutAsync(id, picBin);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        public static object[][] GetInvalidParameters()
        {
            var validPicBin = new byte[] { 1, 2, 3 };

            return new object[][]
            {
                new object[] { "foo", Array.Empty<byte>() },
                new object[] { null, validPicBin },
            };
        }
    }
}