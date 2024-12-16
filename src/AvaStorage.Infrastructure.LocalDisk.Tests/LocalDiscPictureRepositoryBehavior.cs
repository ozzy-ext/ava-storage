using AutoFixture;

namespace AvaStorage.Infrastructure.LocalDisk.Tests
{
    public class LocalFileOperatorBehavior : IDisposable
    {
        private readonly string _basePath = Path.Combine("files", Guid.NewGuid().ToString());

        [Fact]
        public async Task ShouldCreateDirectoryIfNotExists()
        {
            //Arrange
            var fileProvider = new LocalFileOperator(_basePath);
            var picBin = new Fixture().Create<byte[]>();
            var fileName = Guid.NewGuid().ToString();
            var expectedFilePath = Path.Combine(_basePath, fileName);

            //Act
            await fileProvider.WriteFileAsync
            (
                fileName,
                picBin,
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
