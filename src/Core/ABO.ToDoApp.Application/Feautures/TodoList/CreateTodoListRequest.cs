﻿using ABO.ToDoApp.Shared.Models.TodoList;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList;

public class CreateTodoListRequest : IRequest<ActionsResponse<CreateTodoListResponse>>
{
    public string Name { get; set; } = string.Empty;
}
