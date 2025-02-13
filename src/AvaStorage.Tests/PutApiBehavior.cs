using System.Net;
using AvaStorage.Application.UseCases.PutAvatar;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using MyLab.ApiClient.Test;
using MyLab.AvaStorage;
using MyLab.WebErrors;
using Xunit.Abstractions;

namespace AvaStorage.Tests
{
    public class PutApiBehavior : IClassFixture<TestApiFixture<Program, IAvaStorageV1>>
    {
        private readonly TestApiFixture<Program, IAvaStorageV1> _fxt;

        public PutApiBehavior(TestApiFixture<Program, IAvaStorageV1> fxt, ITestOutputHelper output)
        {
            fxt.Output = output;
            _fxt = fxt;
            _fxt.ServiceOverrider = c => c.Configure<ExceptionProcessingOptions>(o => o.HideError = false);
        }

        [Fact]
        public async Task ShouldPutPicture()
        {
            //Arrange
            byte[]? savedPicBin = null;

            var putHandlerMock = new Mock<IRequestHandler<PutAvatarCommand>>();
            putHandlerMock.Setup
            (h =>
                h.Handle
                (
                    It.IsAny<PutAvatarCommand>(),
                    It.IsAny<CancellationToken>()
                )
            ).Callback<PutAvatarCommand, CancellationToken>((c, _) =>
            {
                savedPicBin = c.Picture;
            });

            var outHandlerDescriptor = ServiceDescriptor.Transient(typeof(IRequestHandler<PutAvatarCommand>), s => putHandlerMock.Object);

            var proxyAsset = _fxt.StartWithProxy
                (
                    s => s
                        .Replace(outHandlerDescriptor)
                        .AddSingleton(TestTools.DefaultRepoMock.Object));
            var client = proxyAsset.ApiClient;

            //Act
            var response = await client.PutPngAsync("foo", TestTools.PictureBin);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(TestTools.PictureBin, savedPicBin);
        }

        [Fact]
        public async Task ShouldRejectWrongMimeType()
        {
            //Arrange
            var putHandlerMock = new Mock<IRequestHandler<PutAvatarCommand>>();

            var outHandlerDescriptor = ServiceDescriptor.Transient(typeof(IRequestHandler<PutAvatarCommand>), s => putHandlerMock.Object);

            var proxyAsset = _fxt.StartWithProxy
            (
                s => s
                    .Replace(outHandlerDescriptor)
                    .AddSingleton(TestTools.DefaultRepoMock.Object));
            var client = proxyAsset.ApiClient;

            //Act
            var response = await client.PutWrongFormatAsync("foo", TestTools.PictureBin);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
        }

        [Theory]
        [MemberData(nameof(GetInvalidParameters))]
        public async Task ShouldReturn4ÕõWhenPutBadRequest(string id, byte[] picBin, HttpStatusCode expectedCode)
        {
            //Arrange

            var proxyAsset = _fxt.StartWithProxy
                (
                    s => s.AddSingleton(TestTools.DefaultRepoMock.Object)
                );
            var client = proxyAsset.ApiClient;

            //Act
            var response = await client.PutPngAsync(id, picBin);

            //Assert
            Assert.Equal(expectedCode, response.StatusCode);
        }

        public static object[][] GetInvalidParameters()
        {
            var validPicBin = new byte[] { 1, 2, 3 };
            var tooLargeBin = new byte[600*1024];

            return new object[][]
            {
                new object[] { "foo", Array.Empty<byte>(), HttpStatusCode.BadRequest },
                new object[] { "bar", tooLargeBin, HttpStatusCode.RequestEntityTooLarge },
                new object[] { null, validPicBin, HttpStatusCode.BadRequest },
            };
        }
    }
}