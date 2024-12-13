using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Infrastructure.LocalDisk
{
    public class LocalDiscPictureRepository : IPictureRepository
    {
        private readonly string _personalPath;
        private readonly string _subjectPath;
        private readonly string _defaultPath;

        public LocalDiscPictureRepository(string basePath)
        {
            _personalPath = Path.Combine(basePath, "personal");
            _subjectPath = Path.Combine(basePath, "subjects");
            _defaultPath = Path.Combine(basePath, "default");
        }

        public LocalDiscPictureRepository()
            :this("/var/lib/ava-storage")
        {

        }

        public Task SavePictureAsync(AvatarId id, AvatarPictureBin pictureBin, CancellationToken cancellationToken)
        {
            return File.WriteAllBytesAsync
            (
                Path.Combine(_personalPath, id.Value), 
                pictureBin.Binary.ToArray(), 
                cancellationToken
            );
        }

        public Task<AvatarPictureBin?> LoadOriginalPersonalPictureAsync(AvatarId id, CancellationToken cancellationToken)
        {
            return ReadAvaFromFileAsync(cancellationToken, _personalPath, id.Value, "origin");
        }

        public Task<AvatarPictureBin?> LoadPersonalPictureWithSizeAsync(AvatarId id, int size, CancellationToken cancellationToken)
        {
            return ReadAvaFromFileAsync(cancellationToken, _personalPath, id.Value, size.ToString());
        }

        public Task<AvatarPictureBin?> LoadDefaultSubjectTypePictureAsync(SubjectType subjectType, CancellationToken cancellationToken)
        {
            return ReadAvaFromFileAsync(cancellationToken, _subjectPath, subjectType.Value, "default");
        }

        public Task<AvatarPictureBin?> LoadSubjectTypePictureWithSizeAsync(SubjectType subjectType, int size, CancellationToken cancellationToken)
        {
            return ReadAvaFromFileAsync(cancellationToken, _subjectPath, subjectType.Value, size.ToString());
        }

        public Task<AvatarPictureBin?> LoadDefaultPictureAsync(CancellationToken cancellationToken)
        {
            return ReadAvaFromFileAsync(cancellationToken, _personalPath, _defaultPath, "default");
        }

        public Task<AvatarPictureBin?> LoadDefaultPictureWithSizeAsync(int size, CancellationToken cancellationToken)
        {
            return ReadAvaFromFileAsync(cancellationToken, _personalPath, _defaultPath, size.ToString());
        }

        private static async Task<AvatarPictureBin?> ReadAvaFromFileAsync(CancellationToken cancellationToken, params string[] paths)
        {
            var filePath = Path.Combine(paths);

            if (!File.Exists(filePath))
                return null;

            var fileBin = await File.ReadAllBytesAsync(filePath, cancellationToken);
            return new AvatarPictureBin(fileBin);
        }
    }
}
