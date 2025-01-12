using AvaStorage.Application.Services;
using SixLabors.ImageSharp;
using System.Threading;
using AvaStorage.Domain.Tools;

namespace AvaStorage.Infrastructure.ImageSharp.Services
{
    public class ImageSharpImageMetadataExtractor : IImageMetadataExtractor
    {
        public async Task<ImageMetadata> ExtractAsync(Stream readStream, CancellationToken cancellationToken)
        {
            try
            {
                var imgInfo = await Image.IdentifyAsync(readStream, cancellationToken);

                return new ImageMetadata
                (
                    Width: imgInfo.Width,
                    Height: imgInfo.Height,
                    Format: imgInfo.Metadata.DecodedImageFormat?.Name
                );
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Extract metadata error", e);
            }
        }
    }
}
