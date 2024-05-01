using ABO.ToDoApp.Application.Feautures.TodoItem.Create;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Models;
using ABO.ToDoApp.Shared.Models.TodoItem;
using AutoMapper;

namespace ABO.ToDoApp.Application.MappingProfile;

public class TodoItemProfile : Profile
{
    public TodoItemProfile()
    {
        CreateMap<CreateTodoItemRequest, TodoItem>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Status.Active));

        CreateMap<TodoItem, CreateTodoItemResponse>();

        CreateMap<TodoItemSelect, GetTodoItemsListResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusDecription));

        CreateMap<TodoItem, GetByIdTodoItemResponse>();
    }
}
