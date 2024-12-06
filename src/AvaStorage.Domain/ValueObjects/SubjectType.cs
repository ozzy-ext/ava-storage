using System.ComponentModel.DataAnnotations;

namespace AvaStorage.Domain.ValueObjects;

public record SubjectType
{
    public string Value { get; private set; }

    public SubjectType(string value)
    {
        if (!Validate(value))
            throw new ArgumentException(nameof(value));
        Value = value;
    }

    public static implicit operator SubjectType(string value)
    {
        return new SubjectType(value);
    }
    public static bool TryParse(string value, out SubjectType? id)
    {
        if (!Validate(value))
        {
            id = null;
            return false;
        }

        id = new SubjectType(value);

        return true;
    }

    public static bool Validate(string value)
        => !string.IsNullOrWhiteSpace(value);
}