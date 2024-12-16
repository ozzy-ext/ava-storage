using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Infrastructure.LocalDisk
{
    class LocalDiscPictureRepository : IPictureRepository
    {
        private readonly ILocalFileProvider _fileProvider;

        public LocalDiscPictureRepository(ILocalFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public LocalDiscPictureRepository()
            :this(new LocalFileProvider("/var/lib/ava-storage"))
        {

        }

        public Task SavePictureAsync(IPictureAddressProvider addressProvider, AvatarPictureBin pictureBin, CancellationToken cancellationToken)
        {
            return File.WriteAllBytesAsync
            (
                addressProvider.ProvideAddress(), 
                pictureBin.Binary.ToArray(), 
                cancellationToken
            );
        }

        public async Task<AvatarPictureBin?> LoadPictureAsync(IPictureAddressProvider addressProvider, CancellationToken cancellationToken)
        {
            var filePath = addressProvider.ProvideAddress();
            var fileBin = await _fileProvider.GetFileAsync(filePath, cancellationToken);

            return fileBin != null
                ? new AvatarPictureBin(fileBin)
                : null;
        }
    }
}
