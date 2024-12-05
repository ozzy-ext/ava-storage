using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Infrastructure
{
    public record ImageSize(int Width, int Height)
    {
        public ImageSize(PictureSize pictureSize)
            :this(pictureSize.Width, pictureSize.Height)
        {
            
        }
    }
}
