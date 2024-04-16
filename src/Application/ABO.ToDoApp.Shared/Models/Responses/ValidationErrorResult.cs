namespace ABO.ToDoApp.Shared.Models.Responses;

public class ValidationErrorResult<T> : Result<T>
{
    private readonly Error _error;

    public ValidationErrorResult(Error error)
    {
        _error = error;
    }

    public override ResultType ResultType => ResultType.ValidationError;

    public override Error Errors => _error;

    public override T Data => default!;
}
