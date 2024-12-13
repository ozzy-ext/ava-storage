using System.Formats.Asn1;
using AvaStorage.Application.Tools;
using AvaStorage.Application;
using AvaStorage.Domain.ValueObjects;
using AvaStorage.Infrastructure.ImageSharp.Tools;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AvaStorage.Infrastructure.ImageSharp.Tests
{
    public class ImageResizerBehavior
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
            var picBin = await TestTools.ReadBinFromStreamAsync(mem, CancellationToken.None);
            var originPic = new AvatarPicture
            (
                new AvatarPictureBin(picBin),
                new PictureSize(origin.Width, origin.Height)
            );

            var imgResizer = new ImageResizer(500);

            //Act
            var resultPic = await imgResizer.FitIntoSizeAsync(originPic, CancellationToken.None);

            var resultImage = await LoadImageAsync(resultPic);

            //Assert
            Assert.Equal(500, resultPic.Size.Width);
            Assert.Equal(500, resultPic.Size.Height);

            Assert.Equal(500, resultImage.Width);
            Assert.Equal(500, resultImage.Height);
        }

        private static async Task<ImageInfo> LoadImageAsync(AvatarPicture resultPic)
        {
            var resultMem = new MemoryStream(resultPic.Binary.Binary.ToArray());
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
