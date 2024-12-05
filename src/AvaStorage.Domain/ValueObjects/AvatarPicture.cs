using System.Collections.Immutable;

namespace AvaStorage.Domain.ValueObjects
{
    public record AvatarPicture
    {
        public ImmutableArray<byte> Binary { get; }
        public PictureSize Size { get; }

        public AvatarPicture(byte[] binary, PictureSize size)
        {
            if (!IsValid(binary, size))
                throw new InvalidOperationException("Invalid parameters");

            Binary = binary.ToImmutableArray();
            Size = size;
        }

        public static bool TryLoad(byte[] binary, PictureSize size, out AvatarPicture? picture)
        {
            if (!IsValid(binary, size))
            {
                picture = null;
                return false;
            }

            picture = new AvatarPicture(binary, size);
            return true;
        }

        public static bool IsValid(byte[] binary, PictureSize size)
        {
            return binary != null;
        }
    }
}
