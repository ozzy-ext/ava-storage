namespace AvaStorage.Domain.ValueObjects
{
    public record AvatarId
    {
        public string Value { get; private set; }

        private AvatarId() 
        {
        }

        public static bool TryCreate(string value, out AvatarId? id)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                id = null;
                return false;
            }

            id = new AvatarId
            {
                Value = value
            };

            return false;
        }
    }
}
