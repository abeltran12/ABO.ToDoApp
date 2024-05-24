using ABO.ToDoApp.Contracts;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Shared.Identity.Models;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.Identity.Register;

public class RegisterUserHandler(IAuthService service, IMapper mapper) : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
{
    private readonly IAuthService _service = service;
    private readonly IMapper _mapper = mapper;

    public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var userForCreation = _mapper.Map<User>(request);
        userForCreation.EmailConfirmed = true;

        var response = await _service.RegisterUser(userForCreation, request.Password);

        return response;
    }
}