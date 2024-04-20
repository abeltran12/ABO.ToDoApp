namespace ABO.ToDoApp.Application.Feautures.Identity.Login;

public class TokenResponse
{
    public string? Token { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string? Message { get; set; }
}
