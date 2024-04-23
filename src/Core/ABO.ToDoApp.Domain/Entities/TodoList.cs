namespace ABO.ToDoApp.Domain.Entities;

public class TodoList : IAuditable
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public Status Status { get; set; }

    public ICollection<TodoItem>? TodoItems { get; set; }
}
