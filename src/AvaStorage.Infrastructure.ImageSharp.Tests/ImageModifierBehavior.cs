using AvaStorage.Application;
using AvaStorage.Domain;
using AvaStorage.Domain.ValueObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using AvaStorage.Infrastructure.ImageSharp.Services;

namespace AvaStorage.Infrastructure.ImageSharp.Tests
{
    public class ImageModifierBehavior
    {
        [Theory]
        [MemberData(nameof(GetSizeCases))]
        public async Task ShouldFitToSize(ImageSize origin)
        {
            //Arrange
            var image = new Image<Rgba32>(origin.Width, origin.Height);
            var mem = new MemoryStream();
            await image.SaveAsBmpAsync(mem);

            mem.Seek(0, SeekOrigin.Begin);
            var picBin = await TestTools.ReadBinFromStreamAsync(mem);
            var originPic = new MemoryAvatarFile(picBin);

            var imgResizer = new ImageSharpImageModifier();

            //Act
            var resultPic = await imgResizer.FitIntoSizeAsync(originPic, 500, CancellationToken.None);

            var resultImage = await LoadImageAsync(resultPic);

            //Assert
            Assert.Equal(500, resultImage.Width);
            Assert.Equal(500, resultImage.Height);
        }

        private static async Task<ImageInfo> LoadImageAsync(IAvatarFile resultPic)
        {
            var readStream = resultPic.OpenRead();
            var resultMem = new MemoryStream();
            await readStream.CopyToAsync(resultMem);
            resultMem.Seek(0, SeekOrigin.Begin);

            var resultImage = await Image.IdentifyAsync(resultMem);
            return resultImage;
        }

        public static object[][] GetSizeCases()
        {
            var seeds = new[]
            {
                new ImageSize(200, 100),
                new ImageSize(200, 200),
                new ImageSize(500, 200),
                new ImageSize(600, 200),
                new ImageSize(2000, 1000)
            };

            return seeds.SelectMany(seed =>
            {
                return new object[][]
                {
                    new object[] { seed },
                    new object[]
                    {
                        new ImageSize(seed.Height, seed.Width)
                    }
                };
            }).ToArray();
        }
    }
}
