using AvaStorage.Domain.ValueObjects;
using AvaStorage.Infrastructure.ImageSharp.Tools;

namespace AvaStorage.Infrastructure.ImageSharp.Tests
{
    public class ImageBinaryDeserializerBehavior
    {
        [Theory]
        [InlineData("norm.bmp")]
        [InlineData("norm.gif")]
        [InlineData("norm.jpg")]
        [InlineData("norm.tga")]
        [InlineData("norm.tiff")]
        [InlineData("norm.png")]
        public async Task  ShouldLoadDefiniteFormats(string filename)
        {
            //Arrange
            var filePath = Path.Combine("files", filename);

            //Act
            var picBin = await TestTools.LoadBinFromFileAsync(filePath, CancellationToken.None);
            var pic = await ImageBinaryDeserializer.DeserializeAsync(new AvatarPictureBin(picBin), CancellationToken.None);

            //Assert
            Assert.NotNull(pic);
            Assert.True(pic.Binary is { Binary.Length: > 0 });
            Assert.True(pic.Size is { Height: > 0, Width: >0 });
        }

        [Fact]
        public async Task ShouldNotLoadWrongFormat()
        {
            //Arrange
            const string filePath = "files\\wrong-format.txt";

            //Act
            var picBin = await TestTools.LoadBinFromFileAsync(filePath, CancellationToken.None);
            var pic = await ImageBinaryDeserializer.DeserializeAsync(new AvatarPictureBin(picBin), CancellationToken.None);

            //Assert
            Assert.Null(pic);
        }

        
    }
}