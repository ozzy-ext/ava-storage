using AvaStorage.Domain;

namespace AvaStorage.Infrastructure.LocalDisk
{
    public class LocalAvatarFile : IAvatarFile
    {
        public string Name { get; }

        public string FullPath { get; }

        public DateTimeOffset? LastModified { get; }

        public static bool TryFromFile(string fullPath, out LocalAvatarFile? file)
        {
            if (!File.Exists(fullPath))
            {
                file = null;
                return false;
            }

            file = new LocalAvatarFile(fullPath);
            return true;
        }

        private LocalAvatarFile(string fullPath)
        {
            Name = Path.GetFileName(fullPath);
            FullPath = fullPath;
            LastModified = File.GetLastWriteTimeUtc(fullPath);
        }

        

        public Stream OpenRead()
        {
            return File.OpenRead(FullPath);
        }
    }
}
