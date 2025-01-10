namespace AvaStorage.Infrastructure.LocalDisk
{
    public interface ILocalFileOperator
    {
        Task<byte[]?> ReadFileAsync(string path, CancellationToken cancellationToken);
        Task WriteFileAsync(string path, byte[] data, CancellationToken cancellationToken);
        Task WriteFileAsync(string path, Stream readStream, CancellationToken cancellationToken);
    }
}
