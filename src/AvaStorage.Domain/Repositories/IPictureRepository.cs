using System.Drawing;
using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Repositories
{
    public interface IPictureRepository
    {
        Task SavePictureAsync(AvatarId id, AvatarPicture picture);
        Task<AvatarPicture?> LoadOriginalPersonalPictureAsync(AvatarId id);
        Task<AvatarPicture?> LoadPersonalPictureWithSizeAsync(AvatarId id, int size);
        Task<AvatarPicture?> LoadDefaultSubjectTypePictureAsync(SubjectType subjectType);
        Task<AvatarPicture?> LoadSubjectTypePictureWithSizeAsync(SubjectType subjectType, int size);
        Task<AvatarPicture?> LoadDefaultPictureAsync();
        Task<AvatarPicture?> LoadDefaultPictureWithSizeAsync(int size);
    }
}
