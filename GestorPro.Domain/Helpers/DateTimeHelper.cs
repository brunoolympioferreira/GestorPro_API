namespace GestorPro.Domain.Helpers;

public static class DateTimeHelper
{
    private static readonly TimeZoneInfo BrasiliaZone =
    TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"); // Linux (Docker)

    public static DateTime NowInBrasilia()
        => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, BrasiliaZone);
}
