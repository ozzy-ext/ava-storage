using AvaStorage.Application.Services;
using AvaStorage.Domain.ValueObjects;
using AvaStorage.Infrastructure.ImageSharp.Tools;

namespace AvaStorage.Infrastructure.ImageSharp.Services
{
    public class ImageSharpPictureTools : IPictureTools
    {
        public Task<AvatarPicture?> DeserializeAsync(AvatarPictureBin binary, CancellationToken cancellationToken)
        {
            return ImageBinaryDeserializer.DeserializeAsync(binary, cancellationToken);
        }

        public Task<AvatarPicture> FitIntoSizeAsync(AvatarPicture origin, int targetSize, CancellationToken cancellationToken)
        {
            return new ImageResizer(targetSize).FitIntoSizeAsync(origin, cancellationToken);
        }
    }
}
