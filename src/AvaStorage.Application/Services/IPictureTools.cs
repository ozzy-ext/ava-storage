using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Application.Services
{
    public interface IPictureTools
    {
        Task<AvatarPicture?> DeserializeAsync(AvatarPictureBin binary, CancellationToken cancellationToken);

        Task<AvatarPicture> FitIntoSizeAsync(AvatarPicture origin, int targetSize, CancellationToken cancellationToken);
    }
}
