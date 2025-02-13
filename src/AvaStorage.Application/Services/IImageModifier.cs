using AvaStorage.Domain;

namespace AvaStorage.Application.Services
{
    public interface IImageModifier
    {
        Task<IAvatarFile> FitIntoSizeAsync(IAvatarFile origin, int targetSize, CancellationToken cancellationToken);
        Task<IAvatarFile> ConvertToInnerFormatAsync(IAvatarFile origin, CancellationToken cancellationToken);
    }
}
