namespace AvaStorage.Infrastructure.LocalDisk
{
    public interface ILocalFileOperator
    {
        Stream OpenRead(string path);
        bool IsExist(string path);
        Task WriteFileAsync(string path, byte[] data, CancellationToken cancellationToken);
        Task WriteFileAsync(string path, Stream readStream, CancellationToken cancellationToken);
    }
}
