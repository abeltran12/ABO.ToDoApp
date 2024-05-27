using MediatR;

namespace ABO.ToDoApp.Application.Tests.Unit.Behaviors.Helpers;

public class SampleRequest : IRequest<SampleResponse>
{
    public string Name { get; set; } = string.Empty;
}