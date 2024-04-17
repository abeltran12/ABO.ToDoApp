namespace ABO.ToDoApp.Application.Contracts;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string email);
}
