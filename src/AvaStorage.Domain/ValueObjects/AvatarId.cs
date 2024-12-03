using System.ComponentModel.DataAnnotations;

namespace AvaStorage.Domain.ValueObjects
{
    public record AvatarId
    {
        public string Value { get; private set; }

        public AvatarId(string value)
        {
            if (!Validate(value))
                throw new ArgumentException(nameof(value));
            Value = value;
        }

        public static bool TryParse(string value, out AvatarId? id)
        {
            if (!Validate(value))
            {
                id = null;
                return false;
            }

            id = new AvatarId(value);

            return true;
        }

        public static bool Validate(string value)
            => !string.IsNullOrWhiteSpace(value);
    }
}
