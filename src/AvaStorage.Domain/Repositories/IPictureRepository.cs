using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Repositories
{
    public interface IPictureRepository
    {
        Task SavePictureAsync(AvatarId id, AvatarPicture picture);

        Task LoadPictureAsync(AvatarId id);
    }
}
