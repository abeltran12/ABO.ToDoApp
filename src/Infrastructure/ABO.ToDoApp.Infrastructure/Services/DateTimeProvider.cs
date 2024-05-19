using ABO.ToDoApp.Application.Contracts;

namespace ABO.ToDoApp.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
