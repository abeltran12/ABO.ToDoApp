namespace ABO.ToDoApp.Application.Tests.Unit.Identity.Login;

public class ValidationHelper
{
    private static readonly List<ValidationFailure> _failures =
    [
        new("Email", "Invalid email format."),
        new("Password", "Password length must be 8 characters."),
        new("Email", "Email cannot be empty."),
        new("Password", "Password cannot be null."),
        new("Password", "Password must contain at least one number."),
        new("Email", "Email must be a valid email address.")
    ];

    public static List<ValidationFailure> GetRandomFailures()
    {
        var random = new Random();
        var selectedFailures = new List<ValidationFailure>();
        int numberOfFailures = random.Next(1, _failures.Count + 1);

        for (int i = 0; i < numberOfFailures; i++)
        {
            int index = random.Next(_failures.Count);
            selectedFailures.Add(_failures[index]);
        }

        return selectedFailures;
    }
}