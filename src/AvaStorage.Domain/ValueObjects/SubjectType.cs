namespace AvaStorage.Domain.ValueObjects;

public record SubjectType
{
    public string Value { get; private set; }

    private SubjectType()
    {
    }

    public static bool TryCreate(string value, out SubjectType? id)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            id = null;
            return false;
        }

        id = new SubjectType
        {
            Value = value
        };

        return false;
    }
}