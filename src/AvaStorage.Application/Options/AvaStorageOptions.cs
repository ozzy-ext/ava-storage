namespace AvaStorage.Application.Options
{
    public class AvaStorageOptions
    {
        public int MaxSize { get; set; } = 512;

        public int MaxOriginalFileLength { get; set; } = 512; //Kb
    }
}
