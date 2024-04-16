using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace ABO.ToDoApp.Application.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
        
    }

    public BadRequestException(string message, ValidationResult validation) : base(message)
    {
        Errors = validation.ToDictionary();
    }

    public BadRequestException(string message, IEnumerable<IdentityError> errors) : this(message)
    {
        Errors = errors.GroupBy(e => e.Code)
            .ToDictionary(group => group.Key, 
                group => group.Select(e => e.Description).ToArray());
    }

    public IDictionary<string, string[]>? Errors { get; set; }
}
