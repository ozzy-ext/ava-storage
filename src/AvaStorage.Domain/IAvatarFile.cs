namespace AvaStorage.Domain
{
    public interface IAvatarFile
    {
        public string? Name { get; }
        public DateTimeOffset? LastModified { get; }
        Stream OpenRead();
    }

    public class MemoryAvatarFile : IAvatarFile
    {
        private readonly byte[] _binary;
        public string? Name { get; }
        public DateTimeOffset? LastModified { get; init; }

        public MemoryAvatarFile(byte[] binary, string? name = null)
        {
            _binary = binary;
            Name = name;
        }

        public Stream OpenRead()
        {
            return new MemoryStream(_binary);
        }
    }
}
