using System.Collections.Immutable;

namespace AvaStorage.ByteArrayFormatting;

public static class SupportedMimeTypes
{
    public static ImmutableArray<string> List = new []
    {
        "application/octet-stream",
        "image/png",
        "image/jpeg",
        "image/gif",
        "image/tiff"
    }.ToImmutableArray();
}