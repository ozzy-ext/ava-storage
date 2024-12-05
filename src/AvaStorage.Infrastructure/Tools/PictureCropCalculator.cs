namespace AvaStorage.Infrastructure.Tools
{
    public static class PictureCropCalculator
    {
        public static ImageRectangle CalculateSquare(ImageSize size)
        {
            int x, y;
            int halfDiff = Math.Abs(size.Width - size.Height) / 2;

            if (size.Width > size.Height)
            {
                y = 0;
                x = halfDiff;
            }
            else
            {
                x = 0;
                y = halfDiff;
            }

            var resultSize = Math.Min(size.Width, size.Height);

            return new ImageRectangle(x, y, resultSize, resultSize);
        }
    }
}
