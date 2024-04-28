namespace ABO.ToDoApp.DIC.Models.TodoLists;

public class SelectTodoListParameters
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public string Name { get; set; } = string.Empty;

    public StatusFilter Status { get; set; } = StatusFilter.Active;
}
