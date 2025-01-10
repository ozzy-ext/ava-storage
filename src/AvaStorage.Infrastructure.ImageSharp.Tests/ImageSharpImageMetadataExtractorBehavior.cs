using AvaStorage.Infrastructure.ImageSharp.Services;
using Xunit.Abstractions;

namespace AvaStorage.Infrastructure.ImageSharp.Tests
{
    public class ImageSharpImageMetadataExtractorBehavior
    {
        private readonly ITestOutputHelper _output;
        private readonly ImageSharpImageMetadataExtractor _extractor = new();

        public ImageSharpImageMetadataExtractorBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

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
            await using var stream = File.OpenRead(filePath);

            //Act
            var pic = await _extractor.ExtractAsync(stream, CancellationToken.None);

            _output.WriteLine(pic.Format);

            //Assert
            Assert.NotNull(pic);
            Assert.Equal(436, pic.Width);
            Assert.Equal(395, pic.Height);
        }

        [Fact]
        public async Task ShouldNotLoadWrongFormat()
        {
            //Arrange
            const string filePath = "files\\wrong-format.txt";
            await using var stream = File.OpenRead(filePath);

            //Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _extractor.ExtractAsync(stream, CancellationToken.None));
        }
    }
}