using ABO.ToDoApp.Application.Feautures.TodoList.Create;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Models;
using ABO.ToDoApp.Shared.Models.TodoList;
using AutoMapper;

namespace ABO.ToDoApp.Application.MappingProfile;

public class TodoListProfile : Profile
{
    public TodoListProfile()
    {
        CreateMap<CreateTodoListRequest, TodoList>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Status.Active));

        CreateMap<TodoList, CreateTodoListResponse>();

        CreateMap<TodoListSelect, GetAllTodoListResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusDecription));
    }
}
