namespace AvaStorage.Infrastructure.ImageSharp.Tests;

static class TestTools
{
    public static async Task<byte[]> ReadBinFromStreamAsync(Stream inputStream, CancellationToken cancellationToken)
    {
        var buff = new byte[512];

        var roBuff = new ReadOnlyMemory<byte>(buff);
        var wrBuff = new Memory<byte>(buff);

        await using var mem = new MemoryStream();
        while (inputStream.Position < inputStream.Length)
        {
            int readCount = await inputStream.ReadAsync(wrBuff, cancellationToken);
            await mem.WriteAsync(roBuff.Slice(0, readCount), cancellationToken);
        }

        return mem.ToArray();
    }

    public static async Task<byte[]> LoadBinFromFileAsync(string filename, CancellationToken cancellationToken)
    {
        await using var fileStream = File.OpenRead(filename);
        return await ReadBinFromStreamAsync(fileStream, cancellationToken);
    }
}