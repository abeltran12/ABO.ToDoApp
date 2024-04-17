using ABO.ToDoApp.Application.Contracts;

namespace ABO.ToDoApp.Infrastructure.Email;

public class EmailService : IEmailService
{
    public Task<bool> SendEmailAsync(string email)
    {
        throw new NotImplementedException();
    }
}
