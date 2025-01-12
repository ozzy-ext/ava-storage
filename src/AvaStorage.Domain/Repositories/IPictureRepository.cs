using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Repositories
{
    public interface IPictureRepository
    {
        Task SavePictureAsync(IPictureAddressProvider addressProvider, IAvatarFile file, CancellationToken cancellationToken);
        Task<IAvatarFile?> GetPictureAsync(IPictureAddressProvider addressProvider, CancellationToken cancellationToken);
    }
}
