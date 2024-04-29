using ABO.ToDoApp.Domain.Entities;

namespace ABO.ToDoApp.Domain.Models;

public class TodoListSelect
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Status Status { get; set; }

    public string StatusDecription { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
}
