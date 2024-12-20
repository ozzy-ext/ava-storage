namespace AvaStorage.Domain.Tools
{
    public class PictureSizeValidator
    {
        private readonly int _maxSize;

        public PictureSizeValidator(int maxSize)
        {
            _maxSize = maxSize;
        }

        public bool IsValid(int size)
        {
            return size >= 16 && size <= _maxSize;
        }
    }
}
