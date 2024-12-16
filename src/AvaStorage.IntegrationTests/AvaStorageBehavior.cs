using AvaStorage.Infrastructure.LocalDisk;
using AvaStorage.Tests;
using Microsoft.Extensions.DependencyInjection;
using MyLab.ApiClient.Test;
using MyLab.WebErrors;
using System.Net;
using SixLabors.ImageSharp;
using Xunit.Abstractions;

namespace AvaStorage.IntegrationTests
{
    public class AvaStorageBehavior : IClassFixture<TestApiFixture<Program, IAdminContractV1>>, IClassFixture<TestApiFixture<Program, IPublicContractV1>>
    {
        private readonly TestApiFixture<Program, IAdminContractV1> _fxtAdmin;
        private readonly TestApiFixture<Program, IPublicContractV1> _fxtPublic;

        public AvaStorageBehavior(TestApiFixture<Program, IAdminContractV1> fxtAdmin, TestApiFixture<Program, IPublicContractV1> fxtPublic, ITestOutputHelper output)
        {
            if (Directory.Exists("files"))
                Directory.Delete("files", true);

            fxtAdmin.Output = output;
            fxtPublic.Output = output;
            _fxtAdmin = fxtAdmin;
            _fxtPublic = fxtPublic;
            _fxtPublic.ServiceOverrider = srv => srv.Configure<ExceptionProcessingOptions>(o => o.HideError = false);
            _fxtAdmin.ServiceOverrider = srv => srv.Configure<ExceptionProcessingOptions>(o => o.HideError = false);
        }

        private IAdminContractV1 CreateAdminClient()
        {
            var proxyAsset = _fxtAdmin.StartWithProxy
            (
                s => s.ConfigureLocalDiscPictureStorage(o => o.LocalStoragePath = "files"),
                TestTools.SetAdminPort
            );
            return proxyAsset.ApiClient;
        }

        private IPublicContractV1 CreatePublicClient()
        {
            var proxyAsset = _fxtPublic.StartWithProxy
            (
                s => s.ConfigureLocalDiscPictureStorage(o => o.LocalStoragePath = "files"),
                TestTools.SetPublicPort
            );
            return proxyAsset.ApiClient;
        }

        [Fact]
        public async Task ShouldProvideAvaPicture()
        {
            //Arrange
            var adminClient = CreateAdminClient();
            var publicClient = CreatePublicClient();

            var picBin = await File.ReadAllBytesAsync("norm.png");

            var putResponse = await adminClient.PutAsync("foo", picBin);

            //Act
            var getResponse = await publicClient.GetAsync("foo", 64, "admin");

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
    }
}
