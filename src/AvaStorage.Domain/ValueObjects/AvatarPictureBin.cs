using System.Collections.Immutable;

namespace AvaStorage.Domain.ValueObjects;

public record AvatarPictureBin
{
    public ImmutableArray<byte> Binary { get; }

    public AvatarPictureBin(byte[] binary)
    {
        if (!IsValid(binary))
            throw new InvalidOperationException("Invalid parameters");

        Binary = binary.ToImmutableArray();
    }

    public static bool TryDeserialize(byte[] binary, out AvatarPictureBin? bin)
    {
        if (!IsValid(binary))
        {
            bin = null;
            return false;
        }

        bin = new AvatarPictureBin(binary);
        return true;
    }

    private static bool IsValid(byte[] binary)
    {
        return binary is { Length: > 0 };
    }
}