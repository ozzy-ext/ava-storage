using AvaStorage.Domain;
using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.Repositories;
using Microsoft.Extensions.Options;

namespace AvaStorage.Infrastructure.LocalDisk
{
    class LocalDiscPictureRepository(IOptions<LocalDiskOptions> options) : IPictureRepository
    {
        public async Task SavePictureAsync(IPictureAddressProvider addressProvider, IAvatarFile file, CancellationToken cancellationToken)
        {
            var fileRelAddr = addressProvider.ProvideAddress();
            var fileAbsAddr = ToAbsolutePath(fileRelAddr);

            TouchDirectory(fileAbsAddr);

            await using var avaStream = file.OpenRead();
            await using var destFileStream = File.OpenWrite(fileAbsAddr);

            await avaStream.CopyToAsync(destFileStream, cancellationToken);
        }

        public Task<IAvatarFile?> GetPictureAsync(IPictureAddressProvider addressProvider, CancellationToken cancellationToken)
        {
            var fileRelAddr = addressProvider.ProvideAddress();
            var fileAbsAddr = ToAbsolutePath(fileRelAddr);

            LocalAvatarFile.TryFromFile(fileAbsAddr, out var existFile);

            return Task.FromResult((IAvatarFile?)existFile);
        }
        private string ToAbsolutePath(string fileRelAddr) => Path.Combine(options.Value.LocalStoragePath, fileRelAddr);

        private void TouchDirectory(string filePath)
        {
            var dirName = Path.GetDirectoryName(filePath);

            if (dirName == null)
                throw new InvalidOperationException("Storage directory name not found");

            var dir = new DirectoryInfo(dirName);
            if (!dir.Exists)
                dir.Create();

        }
    }
}
