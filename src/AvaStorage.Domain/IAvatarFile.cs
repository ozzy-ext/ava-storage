namespace AvaStorage.Domain
{
    public interface IAvatarFile
    {
        public string? Name { get; }
        Stream OpenRead();
    }

    public class LocalAvatarFile : IAvatarFile
    {
        private readonly string _fullPath;

        public string? Name { get; }

        public LocalAvatarFile(string name, string fullPath)
        {
            _fullPath = fullPath;
            Name = name;
        }

        public Stream OpenRead()
        {
            return File.OpenRead(_fullPath);
        }
    }

    public class MemoryAvatarFile : IAvatarFile
    {
        private readonly byte[] _binary;
        public string? Name { get; }

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
