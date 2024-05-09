using System.Net;

namespace ABO.ToDoApp.Shared.Models;

public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string ErrorMessage { get; set; }
}
