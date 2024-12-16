namespace AvaStorage.Infrastructure.LocalDisk;

class LocalFileProvider : ILocalFileProvider
{
    private readonly string _basePath;

    public LocalFileProvider(string basePath)
    {
        if (string.IsNullOrWhiteSpace(basePath))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(basePath));
        _basePath = basePath;
    }

    public async Task<byte[]?> GetFileAsync(string path, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_basePath, path);
        if (!File.Exists(filePath))
            return null;

        return await File.ReadAllBytesAsync(filePath, cancellationToken);
    }
}