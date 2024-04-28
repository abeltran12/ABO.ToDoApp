using ABO.ToDoApp.Domain.Entities;

namespace ABO.ToDoApp.Domain.Models;

public class TodoItemSelect
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public Status Status { get; set; }
}