using ABO.ToDoApp.Application.Contracts;
using ABO.ToDoApp.Shared.Constants.GenericMessages;
using MediatR;

namespace ABO.ToDoApp.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILoggerAdapter<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILoggerAdapter<TRequest> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
    {
        _logger.LogInformation(GenericMessageConstants.ExecutingMessage +
            " {RequestType} ", typeof(TRequest));

        var response = await next();

        _logger.LogInformation(GenericMessageConstants.ExecutingMessage + 
            " {RequestType} " + GenericMessageConstants.ProcessedMessage, typeof(TRequest));

        return response;
    }
}