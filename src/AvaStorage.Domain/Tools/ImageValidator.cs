namespace AvaStorage.Domain.Tools
{
    public class ImageValidator
    {
        private readonly int _maxSize;

        public const int MinSize = 16;

        public ImageValidator(int maxSize)
        {
            _maxSize = maxSize;
        }

        public bool IsValid(ImageMetadata imageMetadata)
        {
            return imageMetadata.Height >= MinSize && 
                   imageMetadata.Height <= _maxSize && 
                   imageMetadata.Width >= MinSize && 
                   imageMetadata.Width <= _maxSize;
        }

        public bool IsValidSize(int size)
        {
            return size >= MinSize && size <= _maxSize;
        }
    }

    public record ImageMetadata(int Width, int Height, string? Format);
}
