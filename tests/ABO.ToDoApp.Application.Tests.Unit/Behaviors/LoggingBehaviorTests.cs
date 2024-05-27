using ABO.ToDoApp.Application.Behaviors;
using ABO.ToDoApp.Application.Contracts;
using ABO.ToDoApp.Application.Tests.Unit.Behaviors.Helpers;
using ABO.ToDoApp.Shared.Constants.GenericMessages;

namespace ABO.ToDoApp.Application.Tests.Unit.Behaviors;

public partial class LoggingBehaviorTests
{

    private readonly ILoggerAdapter<SampleRequest> _logger = Substitute.For<ILoggerAdapter<SampleRequest>>();

    [Theory, AutoData]
    public async Task Handle_ShouldLogMessages_WhenInvoked(SampleRequest request)
    {
        // Arrange
        var _sut = new LoggingBehavior<SampleRequest, SampleResponse>(_logger);

        static Task<SampleResponse> next() => Task.FromResult(new SampleResponse());

        // Act
        await _sut.Handle(request, next, CancellationToken.None);

        // Assert
        _logger.Received(1).LogInformation(
            $"{GenericMessageConstants.ExecutingMessage} {{RequestType}} ", typeof(SampleRequest));

        _logger.Received(1).LogInformation(
            $"{GenericMessageConstants.ExecutingMessage} {{RequestType}} {GenericMessageConstants.ProcessedMessage}", typeof(SampleRequest));
    }
}