namespace AvaStorage.Domain.ValueObjects
{
    public record AvatarPicture
    {
        public AvatarPictureBin Binary { get; }
        public PictureSize Size { get; }

        public AvatarPicture(AvatarPictureBin binary, PictureSize size)
        {
            if (!IsValid(binary, size))
                throw new InvalidOperationException("Invalid parameters");

            Binary = binary;
            Size = size;
        }

        public static bool TryLoad(AvatarPictureBin binary, PictureSize size, out AvatarPicture? picture)
        {
            if (!IsValid(binary, size))
            {
                picture = null;
                return false;
            }

            picture = new AvatarPicture(binary, size);
            return true;
        }

        public static bool IsValid(AvatarPictureBin? binary, PictureSize? size)
        {
            return size != null && binary?.Binary is { Length: > 0 };
        }
    }
}
