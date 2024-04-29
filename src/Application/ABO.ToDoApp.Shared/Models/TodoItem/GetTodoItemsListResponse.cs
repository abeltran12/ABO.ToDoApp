namespace ABO.ToDoApp.Shared.Models.TodoItem;

public class GetTodoItemsListResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateOnly Duedate { get; set; }
}
