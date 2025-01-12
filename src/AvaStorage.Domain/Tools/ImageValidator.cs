namespace AvaStorage.Domain.Tools
{
    public class ImageValidator
    {
        private readonly int _maxSize;

        public ImageValidator(int maxSize)
        {
            _maxSize = maxSize;
        }

        public bool IsValid(ImageMetadata imageMetadata)
        {
            return imageMetadata.Height <= _maxSize && imageMetadata.Width <= _maxSize;
        }
    }

    public record ImageMetadata(int Width, int Height, string? Format);
}
