using ABO.ToDoApp.Application.Feautures.Identity;
using ABO.ToDoApp.Domain.Entities;
using AutoMapper;

namespace ABO.ToDoApp.Application.MappingProfile;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserRequest, User>();
    }
}
