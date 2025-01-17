namespace AvaStorage.Application.Options
{
    public class AvaStorageOptions
    {
        public int MaxRequestedSize { get; set; } = 512;

        public int MaxOriginalFileLength { get; set; } = 512; //Kb
    }
}
