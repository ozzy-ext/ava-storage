using AvaStorage.Domain.ValueObjects;

namespace AvaService.Infrastructure
{
    public record ImageSize(int Width, int Height)
    {
        public ImageSize(PictureSize pictureSize)
            :this(pictureSize.Width, pictureSize.Height)
        {
            
        }
    }
}
