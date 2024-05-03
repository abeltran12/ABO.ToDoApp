using ABO.ToDoApp.DIC.Models.TodoLists;

namespace ABO.ToDoApp.DIC.Models.TodoItems;

public class UpdateTodoItem
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateOnly Duedate { get; set; }

    public StatusFilter Status { get; set; }
}
