using ABO.ToDoApp.Application.Contracts;
using ABO.ToDoApp.DIC.Models;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace ABO.ToDoApp.DIC.Middleware
{
    public class GlobalExceptionMiddleware(ILoggerAdapter<GlobalExceptionMiddleware> logger) : IExceptionHandler
    {
        private readonly ILoggerAdapter<GlobalExceptionMiddleware> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
            Exception exception,
            CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
            CustomProblemDetail problem = new();

            switch (exception)
            {
                case Application.Exceptions.BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    problem = new CustomProblemDetail
                    {
                        Title = badRequestException.Message,
                        Status = (int)statusCode,
                        Detail = badRequestException.InnerException?.Message,
                        Type = nameof(Application.Exceptions.BadRequestException),
                        Errors = badRequestException.Errors!,
                    };
                    break;
                case Application.Exceptions.ValidationAppException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    problem = new CustomProblemDetail
                    {
                        Title = validationException.Message,
                        Status = (int)statusCode,
                        Detail = validationException.InnerException?.Message,
                        Type = nameof(Application.Exceptions.ValidationAppException),
                        Errors = validationException.Errors!,
                    };
                    break;
                case Application.Exceptions.NotFoundException NotFound:
                    statusCode = HttpStatusCode.NotFound;
                    problem = new CustomProblemDetail
                    {
                        Title = NotFound.Message,
                        Status = (int)statusCode,
                        Type = nameof(Application.Exceptions.NotFoundException),
                        Detail = NotFound.InnerException?.Message
                    };
                    break;
                default:
                    problem = new CustomProblemDetail
                    {
                        Title = exception.Message,
                        Status = (int)statusCode,
                        Type = nameof(HttpStatusCode.InternalServerError),
                        Detail = exception.StackTrace
                    };
                    break;
            }

            httpContext.Response.StatusCode = (int)statusCode;
            var logMessage = JsonConvert.SerializeObject(problem);
            _logger.LogError(exception, logMessage);
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken: cancellationToken);

            return true;
        }
    }
}