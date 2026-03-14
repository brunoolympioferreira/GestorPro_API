namespace GestorPro.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("E-mail não pode ser vazio.", nameof(value));

        var normalized = value.Trim().ToLowerInvariant();

        if (!normalized.Contains('@') || normalized.Length > 254)
            throw new ArgumentException($"'{value}' não é um e-mail válido.", nameof(value));

        return new Email(normalized);
    }

    public bool Equals(Email? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is Email other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static bool operator ==(Email? a, Email? b) => a?.Equals(b) ?? b is null;
    public static bool operator !=(Email? a, Email? b) => !(a == b);
}
