namespace ABO.ToDoApp.Shared.Models.TodoItem;

public class GetByIdTodoItemResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateOnly Duedate { get; set; }

    public string Status { get; set; } = string.Empty;

    public int TodoListId { get; set; }
}
