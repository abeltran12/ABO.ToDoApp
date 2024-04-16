using Microsoft.AspNetCore.Mvc;

namespace ABO.ToDoApp.DIC.Models;

public class CustomProblemDetail : ProblemDetails
{
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}