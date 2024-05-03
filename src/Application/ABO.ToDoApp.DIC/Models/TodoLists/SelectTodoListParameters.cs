namespace ABO.ToDoApp.DIC.Models.TodoLists;

public class SelectTodoListParameters
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string Name { get; set; } = string.Empty;

    public StatusFilter Status { get; set; } = StatusFilter.Active;
}
