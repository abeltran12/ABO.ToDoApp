namespace ABO.ToDoApp.Shared.Models.Responses;

public abstract class Result<T>
{
    public abstract ResultType ResultType { get; }
    public abstract Error Errors { get; }
    public abstract T Data { get; }
}