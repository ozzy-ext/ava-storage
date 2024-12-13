using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Repositories
{
    public interface IPictureRepository
    {
        Task SavePictureAsync(AvatarId id, AvatarPictureBin pictureBin, CancellationToken cancellationToken);
        Task<AvatarPictureBin?> LoadOriginalPersonalPictureAsync(AvatarId id, CancellationToken cancellationToken);
        Task<AvatarPictureBin?> LoadPersonalPictureWithSizeAsync(AvatarId id, int size, CancellationToken cancellationToken);
        Task<AvatarPictureBin?> LoadDefaultSubjectTypePictureAsync(SubjectType subjectType, CancellationToken cancellationToken);
        Task<AvatarPictureBin?> LoadSubjectTypePictureWithSizeAsync(SubjectType subjectType, int size, CancellationToken cancellationToken);
        Task<AvatarPictureBin?> LoadDefaultPictureAsync(CancellationToken cancellationToken);
        Task<AvatarPictureBin?> LoadDefaultPictureWithSizeAsync(int size, CancellationToken cancellationToken);
    }
}
