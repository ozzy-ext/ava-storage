using AvaStorage.Application;
using AvaStorage.Application.Tools;
using AvaStorage.Domain.ValueObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace AvaStorage.Infrastructure.ImageSharp.Tools
{
    internal class ImageResizer
    {
        public int TargetSize { get; }

        public ImageResizer(int targetSize)
        {
            TargetSize = targetSize;
        }

        public  async Task<AvatarPicture> FitIntoSizeAsync(AvatarPicture origin, CancellationToken cancellationToken)
        {
            using var readMem = new MemoryStream(origin.Binary.Binary.ToArray());
            using var img = await Image.LoadAsync(readMem, cancellationToken);

            if (img.Width == TargetSize && img.Height == TargetSize)
                return origin;

            var newSize = PictureResizeCalculator.Calculate(new ImageSize(img.Width, img.Height), TargetSize);
            var crop = PictureCropCalculator.CalculateSquare(new ImageSize(newSize.Width, newSize.Height));

            img.Mutate(x => x
                .Resize(newSize.Width, newSize.Height)
                .Crop(new Rectangle(crop.X, crop.Y, crop.Width, crop.Height)));

            using var writeMem = new MemoryStream();
            await img.SaveAsync(writeMem, new PngEncoder(), cancellationToken);

            return new AvatarPicture(new AvatarPictureBin(writeMem.ToArray()), new PictureSize(img.Width, img.Height));

        }
    }
}
