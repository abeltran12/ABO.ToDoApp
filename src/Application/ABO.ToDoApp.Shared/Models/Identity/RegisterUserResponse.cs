namespace ABO.ToDoApp.Shared.Identity.Models;

public class RegisterUserResponse
{
    public RegisterUserResponse(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; set; } = string.Empty;

    public string? Message { get; } = "The user was successfully registered";
}
