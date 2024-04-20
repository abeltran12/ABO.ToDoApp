﻿using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Contracts;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Shared.Identity.Models;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.Identity.Register;

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
{
    private readonly IAuthService _service;
    private readonly IMapper _mapper;

    public RegisterUserHandler(IAuthService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var validator = new RegisterUserValidator();
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);

        if (validatorResult.Errors.Count != 0)
            throw new BadRequestException("Register request invalid.", validatorResult);

        var userForCreation = _mapper.Map<User>(request);
        userForCreation.EmailConfirmed = true;

        var response = await _service.RegisterUser(userForCreation, request.Password);

        return response;
    }
}