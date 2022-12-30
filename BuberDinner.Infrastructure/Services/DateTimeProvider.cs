namespace BuberDinner.Infrastructure.Services;

using BuberDinner.Application.Common.Interfaces.Services;

public class DateTimeProvider:IDateTimeProvider
{
    public DateTime UtcNow=> DateTime.UtcNow;
}