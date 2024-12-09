using AvaStorage.Domain.ValueObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace AvaStorage.Infrastructure.ImageSharp;

public partial class ImageSharpPictureTools
{
    public async Task<AvatarPicture?> DeserializeAsync(AvatarPictureBin binary, CancellationToken cancellationToken)
    {
        using var mem = new MemoryStream(binary.Binary.ToArray());

        ImageInfo imgInfo;

        try
        {
            imgInfo = await Image.IdentifyAsync(mem, cancellationToken);
        }
        catch (Exception)
        {
            return null;
        }

        return AvatarPicture.TryLoad(binary, new PictureSize(imgInfo.Width, imgInfo.Height), out var avaPic)
            ? avaPic
            : null;
    }

    public static async Task<AvatarPicture?> LoadFromStreamAsync(Stream inputStream, CancellationToken cancellationToken)
    {
        Image img;

        try
        {
            img = await Image.LoadAsync(inputStream, cancellationToken);
        }
        catch (Exception)
        {
            return null;
        }

        using var mem = new MemoryStream();
        await img.SaveAsync(mem, new PngEncoder(), CancellationToken.None);
        img.Dispose();

        return AvatarPicture.TryLoad(new AvatarPictureBin(mem.ToArray()), new PictureSize(img.Width, img.Height), out var avaPic)
            ? avaPic
            : null;
    }

    public static async Task<AvatarPicture?> LoadFromFileAsync(string filename, CancellationToken cancellationToken)
    {
        await using var fileStream = File.OpenRead(filename);
        return await LoadFromStreamAsync(fileStream, cancellationToken);
    }
}