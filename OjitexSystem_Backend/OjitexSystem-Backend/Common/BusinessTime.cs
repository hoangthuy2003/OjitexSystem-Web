namespace OjitexSystem_Backend.Common;

public static class BusinessTime
{
    public static DateTime GetBusinessNow(this TimeProvider timeProvider) =>
        timeProvider.GetUtcNow().ToOffset(TimeSpan.FromHours(7)).DateTime;
}
