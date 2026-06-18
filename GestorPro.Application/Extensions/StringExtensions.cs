namespace GestorPro.Application.Extensions;

public static class StringExtensions
{
    public static string Normalize(this string value)
    => value?.Trim().ToLower().Replace(" ", string.Empty) ?? string.Empty;
}
