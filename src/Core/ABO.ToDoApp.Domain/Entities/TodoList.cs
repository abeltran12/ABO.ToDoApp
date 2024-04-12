namespace ABO.ToDoApp.Domain.Entities;

public class TodoList
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int UserId { get; set; }

    public Status Status { get; set; }

    public ICollection<TodoItem>? TodoItems { get; set; }
}
