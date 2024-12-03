using System.ComponentModel.DataAnnotations;

namespace ValueObjects
{
    public record NonEmptyString
    {
        public string Value { get; private set; }

        public NonEmptyString(string value)
        {
            if (!IsValid(value))
                throw new ArgumentException("Avatar ID has wrong format");

            Value = value;
        }

        public static bool IsValid(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
