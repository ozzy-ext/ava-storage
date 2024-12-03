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
    public class AvaStorageApiBehavior : IClassFixture<TestApiFixture<Program, IAvaStorageContractV1>>
    {
        private readonly TestApiFixture<Program, IAvaStorageContractV1> _fxt;

        public AvaStorageApiBehavior(TestApiFixture<Program, IAvaStorageContractV1> fxt, ITestOutputHelper output)
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

            var proxyAsset = _fxt.StartWithProxy(s => s.Replace(outHandlerDescriptor));
            var client = proxyAsset.ApiClient;

            var pictureBin = new byte[] { 1, 2, 3 };
            //Act
            var response = await client.PutAsync("foo", "bar", pictureBin);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            putHandlerMock.Verify(h => h.Handle
                (
                    It.Is<PutAvatarCommand>(c => 
                            c.Id == "foo" &&
                            c.SubjectType == "bar" &&
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
        public async Task ShouldReturn400WhenBadRequest(string id, string subType, byte[] picBin)
        {
            //Arrange
            var repoMock = new Mock<IPictureRepository>();

            var proxyAsset = _fxt.StartWithProxy(s => s.AddSingleton(repoMock.Object));
            var client = proxyAsset.ApiClient;

            //Act
            var response = await client.PutAsync(id, subType, picBin);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        public static object[][] GetInvalidParameters()
        {
            var validPicBin = new byte[] { 1, 2, 3 };

            return new object[][]
            {
                new object[] { "foo", "bar", Array.Empty<byte>() },
                new object[] { null, "bar", validPicBin },
            };
        }
    }
}