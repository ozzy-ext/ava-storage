using SixLabors.ImageSharp;

namespace AvaStorage.Domain.ValueObjects
{
    public record AvatarPicture : IDisposable
    {
        public Image Image { get; private set; }

        private AvatarPicture(Image image)
        {
            Image = image ?? throw new ArgumentNullException(nameof(image));
        }

        public static bool TryLoad(byte[] binary, out AvatarPicture? picture)
        {
            Image image;
            try
            {
                image = Image.Load(binary);
            }
            catch (Exception)
            {
                picture = null;
                return false;
            }

            picture = new AvatarPicture(image);

            return true;
        }

        public void Dispose()
        {
            Image.Dispose();
        }
    }
}
