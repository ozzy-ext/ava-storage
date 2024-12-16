using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Repositories
{
    public interface IPictureRepository
    {
        Task SavePictureAsync(IPictureAddressProvider addressProvider, AvatarPictureBin pictureBin, CancellationToken cancellationToken);
        Task<AvatarPictureBin?> LoadPictureAsync(IPictureAddressProvider addressProvider, CancellationToken cancellationToken);
    }
}
