using ABO.ToDoApp.Domain.Entities;

namespace ABO.ToDoApp.Domain.RequestFilters;

public class TodoListParameters : RequestParameters
{
    public string? Name { get; set; }

    public Status Status { get; set; } = Status.Active;
}
