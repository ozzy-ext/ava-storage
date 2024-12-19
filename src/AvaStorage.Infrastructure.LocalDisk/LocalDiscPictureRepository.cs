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
        private readonly IDslLogger? _logger;

        public LocalDiscPictureRepository(ILocalFileOperator fileOperator, ILogger<LocalDiscPictureRepository>? logger = null)
        {
            _fileOperator = fileOperator;
            _logger = logger?.Dsl();
        }

        public LocalDiscPictureRepository(IOptions<LocalDiskOptions> options, ILogger<LocalDiscPictureRepository>? logger = null)
            :this(new LocalFileOperator(options.Value.LocalStoragePath), logger)
        {

        }

        public async Task SavePictureAsync(IPictureAddressProvider addressProvider, AvatarPictureBin pictureBin, CancellationToken cancellationToken)
        {
            var fileAddr = addressProvider.ProvideAddress();
            
            await _fileOperator.WriteFileAsync
            (
                fileAddr, 
                pictureBin.Binary.ToArray(), 
                cancellationToken
            );

            _logger?.Debug("Picture saved")
                .AndFactIs("file", fileAddr)
                .AndFactIs("size", pictureBin.Binary.Length)
                .Write();
        }

        public async Task<AvatarPictureBin?> LoadPictureAsync(IPictureAddressProvider addressProvider, CancellationToken cancellationToken)
        {
            var filePath = addressProvider.ProvideAddress();
            var fileBin = await _fileOperator.ReadFileAsync(filePath, cancellationToken);

            var logRec = _logger?.Debug("Picture loaded")
                .AndFactIs("file", filePath);

            if (fileBin != null)
            {
                logRec = logRec?.AndFactIs("size", fileBin.Length)
                    .AndLabel("found");
            }
            else
            {
                logRec = logRec?.AndLabel("not-found");
            }

            logRec?.Write();

            return fileBin != null
                ? new AvatarPictureBin(fileBin)
                : null;
        }
    }
}
