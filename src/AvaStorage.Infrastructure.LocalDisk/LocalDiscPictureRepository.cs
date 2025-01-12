using System.Security.AccessControl;
using AvaStorage.Domain;
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

        public Task SavePictureAsync(IPictureAddressProvider addressProvider, IAvatarFile file, CancellationToken cancellationToken)
        {
            var fileAddr = addressProvider.ProvideAddress();

            using var stream = file.OpenRead();

            return _fileOperator.WriteFileAsync
            (
                fileAddr,
                stream, 
                cancellationToken
            );
        }

        public Task<IAvatarFile?> GetPictureAsync(IPictureAddressProvider addressProvider, CancellationToken cancellationToken)
        {
            var filePath = addressProvider.ProvideAddress();

            if (!_fileOperator.IsExist(filePath))
                return Task.FromResult((IAvatarFile?)null);

            var filename = Path.GetFileName(filePath);
            
            return Task.FromResult((IAvatarFile?)new LocalAvatarFile(filename, filePath, _fileOperator));
        }
    }
}
