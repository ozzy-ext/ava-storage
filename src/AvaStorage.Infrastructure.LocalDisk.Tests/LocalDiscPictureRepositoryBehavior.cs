using AutoFixture;
using AvaStorage.Domain;
using AvaStorage.Domain.PictureAddressing;
using Microsoft.Extensions.Options;
using Moq;

namespace AvaStorage.Infrastructure.LocalDisk.Tests
{
    public class LocalDiscPictureRepositoryBehavior : IDisposable
    {
        private readonly string _basePath = Path.Combine("files", Guid.NewGuid().ToString());

        [Fact]
        public async Task ShouldCreateDirectoryIfDoesNotExists()
        {
            //Arrange
            var options = new LocalDiskOptions
            {
                LocalStoragePath = _basePath
            };
            var repo = new LocalDiscPictureRepository(new OptionsWrapper<LocalDiskOptions>(options));
            
            var picBin = new Fixture().Create<byte[]>();
            var avaFile = new MemoryAvatarFile(picBin);

            var fileName = Guid.NewGuid().ToString();
            var expectedFilePath = Path.Combine(_basePath, fileName);

            var addrProviderMock = new Mock<IPictureAddressProvider>();
            addrProviderMock.Setup(p => p.ProvideAddress())
                .Returns(() => fileName);

            //Act
            await repo.SavePictureAsync
            (
                addrProviderMock.Object,
                avaFile,
                CancellationToken.None
            );

            //Assert
            Assert.True(File.Exists(expectedFilePath));
        }

        public void Dispose()
        {
            if(Directory.Exists(_basePath))
                Directory.Delete(_basePath, true);
        }
    }
}
