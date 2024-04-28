﻿namespace ABO.ToDoApp.Shared.Models.TodoList;

public class GetAllTodoListResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}