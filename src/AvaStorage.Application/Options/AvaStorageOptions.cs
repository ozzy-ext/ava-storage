namespace AvaStorage.Application.Options
{
    public class AvaStorageOptions
    {
        public int MaxRequestedSize { get; set; } = 512; //Pix

        public int MaxOriginalFileLength { get; set; } = 512; //Kb

        public int[]? PredefinedSizes { get; set; }
    }
}
