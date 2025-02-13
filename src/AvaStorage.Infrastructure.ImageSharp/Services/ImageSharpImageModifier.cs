using AvaStorage.Application;
using AvaStorage.Application.Services;
using AvaStorage.Application.Tools;
using AvaStorage.Domain;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace AvaStorage.Infrastructure.ImageSharp.Services
{
    public class ImageSharpImageModifier : IImageModifier
    {
        public async Task<IAvatarFile> FitIntoSizeAsync(IAvatarFile origin, int targetSize, CancellationToken cancellationToken)
        {
            await using var stream = origin.OpenRead();
            using var img = await Image.LoadAsync(stream, cancellationToken);

            if (img.Width == targetSize && img.Height == targetSize)
                return origin;

            var newSize = PictureResizeCalculator.Calculate(new ImageSize(img.Width, img.Height), targetSize);
            var crop = PictureCropCalculator.CalculateSquare(new ImageSize(newSize.Width, newSize.Height));

            img.Mutate(x => x
                .Resize(newSize.Width, newSize.Height)
                .Crop(new Rectangle(crop.X, crop.Y, crop.Width, crop.Height)));

            using var writeMem = new MemoryStream();

            if (img.Metadata.DecodedImageFormat != null)
            {
                await img.SaveAsync(writeMem, img.Metadata.DecodedImageFormat, cancellationToken);
            }
            else
            {
                await img.SaveAsync(writeMem, new PngEncoder(), cancellationToken);
            }

            return new MemoryAvatarFile(writeMem.ToArray());
        }

        public async Task<IAvatarFile> ConvertToInnerFormatAsync(IAvatarFile origin, CancellationToken cancellationToken)
        {
            await using var stream = origin.OpenRead();
            using var img = await Image.LoadAsync(stream, cancellationToken);
            
            await using var writeMem = new MemoryStream();

            await img.SaveAsync(writeMem, new PngEncoder(), cancellationToken);

            return new MemoryAvatarFile(writeMem.ToArray());
        }
    }
}
