using AvaStorage.Infrastructure.LocalDisk;
using AvaStorage.Tests;
using Microsoft.Extensions.DependencyInjection;
using MyLab.ApiClient.Test;
using MyLab.WebErrors;
using System.Net;
using MyLab.AvaStorage;
using SixLabors.ImageSharp;
using Xunit.Abstractions;

namespace AvaStorage.IntegrationTests
{
    public class AvaStorageBehavior : IClassFixture<TestApiFixture<Program, IAvaStorageV1>>, IDisposable
    {
        private readonly TestApiFixture<Program, IAvaStorageV1> _fxt;

        public AvaStorageBehavior(TestApiFixture<Program, IAvaStorageV1> fxt, ITestOutputHelper output)
        {
            if (Directory.Exists("files"))
                Directory.Delete("files", true);

            fxt.Output = output;
            _fxt = fxt;
            _fxt.ServiceOverrider = srv => srv.Configure<ExceptionProcessingOptions>(o => o.HideError = false);
        }

        private IAvaStorageV1 CreateClient()
        {
            var proxyAsset = _fxt.StartWithProxy
            (
                s => s.ConfigureLocalDiscPictureStorage(o => o.LocalStoragePath = "files")
            );
            return proxyAsset.ApiClient;
        }

        [Fact]
        public async Task ShouldProvideAvaPicture()
        {
            //Arrange
            var client = CreateClient();

            var picBin = await File.ReadAllBytesAsync("norm.png");

            var putResponse = await client.PutPngAsync("foo", picBin);

            //Act
            var getResponse = await client.GetAsync("foo", 64, "admin");

            var loadedImg = await ParseImageAsync(getResponse.ResponseContent);

            //Assert
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.NotNull(loadedImg);
            Assert.Equal(64, loadedImg.Width);
            Assert.Equal(64, loadedImg.Height);
        }

        private async Task<Image?> ParseImageAsync(byte[]? getResponseResponseContent)
        {
            if (getResponseResponseContent == null) return null;

            await using var mem = new MemoryStream(getResponseResponseContent);
            return await Image.LoadAsync(mem);
        }

        public void Dispose()
        {
            if (Directory.Exists("files"))
                Directory.Delete("files", true);
        }
    }
}
