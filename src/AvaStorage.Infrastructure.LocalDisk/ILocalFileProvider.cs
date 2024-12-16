namespace AvaStorage.Infrastructure.LocalDisk
{
    public interface ILocalFileProvider
    {
        Task<byte[]?> GetFileAsync(string path, CancellationToken cancellationToken);
    }
}
