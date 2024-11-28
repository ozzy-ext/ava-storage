using System.Collections.Immutable;

namespace AvaStorage.Domain.ValueObjects
{
    public record OriginAvatarPicture
    {
        public ImmutableArray<byte> Value { get; private set; }

        private OriginAvatarPicture()
        {
        }

        public static bool TryCreate(byte[] binary, out OriginAvatarPicture? id)
        {
            id = new OriginAvatarPicture
            {
                Value = value
            };

            return false;
        }
    }
}
