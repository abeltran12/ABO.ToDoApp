namespace ABO.ToDoApp.Shared.Models.Responses;

public class Error
{
    public Error()
    {
        
    }

    public Error(Dictionary<string, string>? issues, string message)
    {
        Issues = issues;
        Message = message;
    }

    public IDictionary<string, string>? Issues;

    public string Message { get; set; } = string.Empty;
}
