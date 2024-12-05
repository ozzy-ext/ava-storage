using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Tools
{
    public class PictureValidator
    {
        private readonly int _maxSize;

        public PictureValidator(int maxSize)
        {
            _maxSize = maxSize;
        }

        public bool IsValid(AvatarPicture avatarPicture)
        {
            return avatarPicture.Size.Height <= _maxSize && avatarPicture.Size.Width <= _maxSize;
        }
    }
}
