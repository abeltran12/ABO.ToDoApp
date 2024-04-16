namespace ABO.ToDoApp.Shared.Identity.Models;

public record LoginUserResponse
{
    public string? UserName { get; init; }
    public string? Password { get; init; }
}
