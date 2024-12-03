using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Repositories
{
    public interface IPictureRepository
    {
        Task SavePictureAsync(AvatarId id, SubjectType subjectType, AvatarPicture picture);
    }
}
