using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Domain.Tools
{
    public class PictureValidator
    {
        public int MaxPictureSize { get; set; } = 512;

        public bool IsValid(AvatarPicture avatarPicture)
        {
            return avatarPicture.Image.Height <= MaxPictureSize && avatarPicture.Image.Width <= MaxPictureSize;
        }
    }
}
