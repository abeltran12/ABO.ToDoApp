namespace ABO.ToDoApp.Shared.Models.TodoList;

public class ActionsResponse<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }
}
