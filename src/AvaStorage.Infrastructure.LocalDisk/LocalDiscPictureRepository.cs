using System.Security.AccessControl;
using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;
using Microsoft.Extensions.Options;

namespace AvaStorage.Infrastructure.LocalDisk
{
    class LocalDiscPictureRepository : IPictureRepository
    {
        private readonly ILocalFileOperator _fileOperator;

        public LocalDiscPictureRepository(ILocalFileOperator fileOperator)
        {
            _fileOperator = fileOperator;
        }

        public LocalDiscPictureRepository(IOptions<LocalDiskOptions> options)
            :this(new LocalFileOperator(options.Value.LocalStoragePath))
        {

        }

        public Task SavePictureAsync(IPictureAddressProvider addressProvider, AvatarPictureBin pictureBin, CancellationToken cancellationToken)
        {
            var fileAddr = addressProvider.ProvideAddress();
            
            return _fileOperator.WriteFileAsync
            (
                fileAddr, 
                pictureBin.Binary.ToArray(), 
                cancellationToken
            );
        }

        public async Task<AvatarPictureBin?> LoadPictureAsync(IPictureAddressProvider addressProvider, CancellationToken cancellationToken)
        {
            var filePath = addressProvider.ProvideAddress();
            var fileBin = await _fileOperator.ReadFileAsync(filePath, cancellationToken);

            return fileBin != null
                ? new AvatarPictureBin(fileBin)
                : null;
        }
    }
}
