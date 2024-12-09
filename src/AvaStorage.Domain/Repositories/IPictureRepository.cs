using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Repositories
{
    public interface IPictureRepository
    {
        Task SavePictureAsync(AvatarId id, AvatarPictureBin pictureBin);
        Task<AvatarPictureBin?> LoadOriginalPersonalPictureAsync(AvatarId id);
        Task<AvatarPictureBin?> LoadPersonalPictureWithSizeAsync(AvatarId id, int size);
        Task<AvatarPictureBin?> LoadDefaultSubjectTypePictureAsync(SubjectType subjectType);
        Task<AvatarPictureBin?> LoadSubjectTypePictureWithSizeAsync(SubjectType subjectType, int size);
        Task<AvatarPictureBin?> LoadDefaultPictureAsync();
        Task<AvatarPictureBin?> LoadDefaultPictureWithSizeAsync(int size);
    }
}
