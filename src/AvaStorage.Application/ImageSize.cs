using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Application
{
    public record ImageSize(int Width, int Height)
    {
        public ImageSize(PictureSize pictureSize)
            :this(pictureSize.Width, pictureSize.Height)
        {
            
        }
    }
}
