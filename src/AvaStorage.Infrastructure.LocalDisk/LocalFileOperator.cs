using MyLab.Log.Dsl;

namespace AvaStorage.Infrastructure.LocalDisk;

class LocalFileOperator : ILocalFileOperator
{
    private readonly string _basePath;

    public IDslLogger? Logger { get; set; }

    public LocalFileOperator(string basePath)
    {
        if (string.IsNullOrWhiteSpace(basePath))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(basePath));
        _basePath = basePath;
    }

    public Stream OpenRead(string path)
    {
        var filePath = Path.Combine(_basePath, path);

        return File.OpenRead(filePath);
    }

    public bool IsExist(string path)
    {
        return File.Exists(Path.Combine(_basePath, path));
    }

    public async Task WriteFileAsync(string path, byte[] data, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_basePath, path);

        TouchDirectory(filePath);

        await File.WriteAllBytesAsync(filePath, data, cancellationToken);

        WriteLogAboutWrittenFile(filePath, data.Length);
    }

    public async Task WriteFileAsync(string path, Stream readStream, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_basePath, path);

        TouchDirectory(filePath);

        await using var outputStream = File.OpenWrite(filePath);
        await readStream.CopyToAsync(outputStream, cancellationToken);

        WriteLogAboutWrittenFile(filePath, outputStream.Length);
    }

    void WriteLogAboutWrittenFile(string path, long length)
    {
        Logger?.Debug("File was written")
            .AndFactIs("file", path)
            .AndFactIs("size", length)
            .Write();
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