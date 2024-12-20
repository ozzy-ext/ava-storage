using AvaStorage.Domain.ValueObjects;
using SixLabors.ImageSharp;

namespace AvaStorage.Infrastructure.ImageSharp.Tools
{
    internal static class ImageBinaryDeserializer
    {
        public static async Task<AvatarPicture?> DeserializeAsync(AvatarPictureBin binary, CancellationToken cancellationToken)
        {
            using var mem = new MemoryStream(binary.Binary.ToArray());

            ImageInfo imgInfo;

            try
            {
                imgInfo = await Image.IdentifyAsync(mem, cancellationToken);
            }
            catch (Exception)
            {
                return null;
            }

            return AvatarPicture.TryLoad(binary, new PictureSize(imgInfo.Width, imgInfo.Height), out var avaPic)
                ? avaPic
                : null;
        }
    }
}
