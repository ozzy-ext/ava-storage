namespace AvaStorage.Domain
{
    public interface IAvatarFile
    {
        public DateTimeOffset? LastModified { get; }
        Stream OpenRead();
    }

    public class MemoryAvatarFile : IAvatarFile
    {
        private readonly byte[] _binary;
        public DateTimeOffset? LastModified { get; init; }

        public MemoryAvatarFile(byte[] binary)
        {
            _binary = binary;
        }

        public Stream OpenRead()
        {
            return new MemoryStream(_binary);
        }
    }
}
