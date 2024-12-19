using System.Security.AccessControl;
using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyLab.Log.Dsl;

namespace AvaStorage.Infrastructure.LocalDisk
{
    class LocalDiscPictureRepository : IPictureRepository
    {
        private readonly ILocalFileOperator _fileOperator;
        public LocalDiscPictureRepository(ILocalFileOperator fileOperator, ILogger<LocalDiscPictureRepository>? logger = null)
        {
            _fileOperator = fileOperator;
        }

        public LocalDiscPictureRepository(IOptions<LocalDiskOptions> options, ILogger<LocalDiscPictureRepository>? logger = null)
            :this(new LocalFileOperator(options.Value.LocalStoragePath){ Logger = logger.Dsl() }, logger)
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
