using AvaStorage.Domain.Tools;

namespace AvaStorage.Application.Services;

public interface IImageMetadataExtractor
{
    Task<ImageMetadata> ExtractAsync(Stream readStream, CancellationToken cancellationToken);
}