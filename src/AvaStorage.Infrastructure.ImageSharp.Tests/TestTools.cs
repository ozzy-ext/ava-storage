namespace AvaStorage.Infrastructure.ImageSharp.Tests;

static class TestTools
{
    public static async Task<byte[]> ReadBinFromStreamAsync(Stream inputStream)
    {
        using var mem = new MemoryStream();
        await inputStream.CopyToAsync(mem);

        return mem.ToArray();
    }

    public static async Task<byte[]> LoadBinFromFileAsync(string filename)
    {
        await using var fileStream = File.OpenRead(filename);
        return await ReadBinFromStreamAsync(fileStream);
    }
}