namespace AvaStorage.Application.Tools
{
    public static class PictureResizeCalculator
    {
        public static ImageSize Calculate(ImageSize originSize, int targetSize)
        {
            int newWidth, newHeight;

            if (originSize.Width > originSize.Height)
            {
                newHeight = targetSize;
                newWidth = originSize.Width * newHeight / originSize.Height;
            }
            else
            {
                newWidth = targetSize;
                newHeight = originSize.Height * newWidth / originSize.Width;
            }

            return new ImageSize(newWidth, newHeight);
        }
    }
}
