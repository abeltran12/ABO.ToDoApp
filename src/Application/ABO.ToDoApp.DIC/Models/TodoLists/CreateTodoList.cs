﻿namespace ABO.ToDoApp.DIC.Models.TodoLists;

public class CreateTodoList
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
