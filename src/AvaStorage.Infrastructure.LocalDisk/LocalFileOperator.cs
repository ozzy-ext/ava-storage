namespace AvaStorage.Infrastructure.LocalDisk;

class LocalFileOperator : ILocalFileOperator
{
    private readonly string _basePath;

    public LocalFileOperator(string basePath)
    {
        if (string.IsNullOrWhiteSpace(basePath))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(basePath));
        _basePath = basePath;
    }

    public async Task<byte[]?> ReadFileAsync(string path, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_basePath, path);
        if (!File.Exists(filePath))
            return null;

        return await File.ReadAllBytesAsync(filePath, cancellationToken);
    }

    public Task WriteFileAsync(string path, byte[] data, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_basePath, path);

        TouchDirectory(filePath);

        return File.WriteAllBytesAsync(filePath, data, cancellationToken);
    }

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