using AvaStorage.Domain;

namespace AvaStorage.Infrastructure.LocalDisk
{
    public class LocalAvatarFile : IAvatarFile
    {
        private readonly string _path;
        private readonly ILocalFileOperator _fileOperator;

        public string? Name { get; }

        public LocalAvatarFile(string name, string path, ILocalFileOperator fileOperator)
        {
            _path = path;
            _fileOperator = fileOperator;
            Name = name;
        }

        public Stream OpenRead()
        {
            return _fileOperator.OpenRead(_path);
        }
    }
}
