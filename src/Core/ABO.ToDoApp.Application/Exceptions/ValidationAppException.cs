using Microsoft.AspNetCore.Identity;

namespace ABO.ToDoApp.Application.Exceptions;

public class ValidationAppException : Exception
{
    public ValidationAppException(string message) : base(message)
    {

    }

    /// <summary>
    /// Este metodo es para los errores de fluent validation
    /// </summary>
    /// <param name="message"></param>
    /// <param name="validation"></param>
    public ValidationAppException(IDictionary<string, string[]> errors) 
        : base("One or more validation errors ocurred")
    {
        Errors = errors;
    }

    /// <summary>
    /// Este metodo es para los errores de identity
    /// </summary>
    /// <param name="message"></param>
    /// <param name="errors"></param>
    public ValidationAppException(IEnumerable<IdentityError> errors) 
        : this("One or more validation errors ocurred")
    {
        Errors = errors.GroupBy(e => e.Code)
            .ToDictionary(group => group.Key,
                group => group.Select(e => e.Description).ToArray());
    }

    public IDictionary<string, string[]>? Errors { get; set; }
}
