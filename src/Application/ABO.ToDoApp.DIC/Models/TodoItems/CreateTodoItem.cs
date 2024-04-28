namespace ABO.ToDoApp.DIC.Models.TodoItems;

public class CreateTodoItem
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateOnly Duedate { get; set; }
}
