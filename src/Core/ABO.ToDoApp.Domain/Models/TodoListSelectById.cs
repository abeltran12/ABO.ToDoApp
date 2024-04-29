namespace ABO.ToDoApp.Domain.Models;

public class TodoListSelectById
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string StatusDecription { get; set; } = string.Empty;
}
