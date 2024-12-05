namespace AvaStorage.Domain.ValueObjects
{
    public record PictureSize
    {
        public int Width { get; }
        public int Height { get; }

        public PictureSize(int width, int height)
        {
            if (!IsValid(width, height))
                throw new InvalidOperationException("Invalid parameters");

            Width = width;
            Height = height;
        }

        public static bool TryCreate(int width, int height, out PictureSize? size)
        {
            if (!IsValid(width, height))
            {
                size = null;
                return false;
            }

            size = new PictureSize(width, height);
            return true;
        }

        public static bool IsValid(int width, int height)
        {
            return width > 0 && height > 0;
        }
    }
}
