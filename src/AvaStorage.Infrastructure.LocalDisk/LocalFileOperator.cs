using MyLab.Log.Dsl;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public async Task<byte[]?> ReadFileAsync(string path, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_basePath, path);
        if (!File.Exists(filePath))
        {
            Logger?.Debug("File for reading not found")
                .AndFactIs("file", filePath)
                .Write();

            return null;
        }

        var bin = await File.ReadAllBytesAsync(filePath, cancellationToken);

        Logger?.Debug("File read")
            .AndFactIs("file", filePath)
            .AndFactIs("size", bin.Length)
            .Write();

        return bin;
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

        var outputStream = File.OpenWrite(filePath);
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