namespace ABO.ToDoApp.Domain.Entities;

public class TodoItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateOnly Duedate { get; set; }

    public Status Status { get; set; }

    public int IdTodoList { get; set; }

    public TodoList? TodoList { get; set; }
}
