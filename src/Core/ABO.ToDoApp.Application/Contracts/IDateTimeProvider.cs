namespace ABO.ToDoApp.Application.Contracts;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
