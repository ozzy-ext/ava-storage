using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Infrastructure.LocalDisk
{
    public class LocalDiscPictureRepository : IPictureRepository
    {
        public Task SavePictureAsync(AvatarId id, AvatarPictureBin pictureBin)
        {
            throw new NotImplementedException();
        }

        public Task<AvatarPictureBin?> LoadOriginalPersonalPictureAsync(AvatarId id)
        {
            throw new NotImplementedException();
        }

        public Task<AvatarPictureBin?> LoadPersonalPictureWithSizeAsync(AvatarId id, int size)
        {
            throw new NotImplementedException();
        }

        public Task<AvatarPictureBin?> LoadDefaultSubjectTypePictureAsync(SubjectType subjectType)
        {
            throw new NotImplementedException();
        }

        public Task<AvatarPictureBin?> LoadSubjectTypePictureWithSizeAsync(SubjectType subjectType, int size)
        {
            throw new NotImplementedException();
        }

        public Task<AvatarPictureBin?> LoadDefaultPictureAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AvatarPictureBin?> LoadDefaultPictureWithSizeAsync(int size)
        {
            throw new NotImplementedException();
        }
    }
}
