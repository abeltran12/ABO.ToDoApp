namespace ABO.ToDoApp.Shared.Models.TodoItem;

public class CreateTodoItemResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateOnly Duedate { get; set; }
}
