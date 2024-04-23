using ABO.ToDoApp.Application.Feautures.TodoList;
using ABO.ToDoApp.Domain.Entities;
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
    }
}
