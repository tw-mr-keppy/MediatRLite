using MediatRLite.Core.Commands;

namespace MediatR.Application;

public class HelloWorldCommandHandler : ICommandHandler<HelloWorldCommand, string>
{
    public async Task<string> HandleAsync(HelloWorldCommand command, CancellationToken cancellationToken = default)
    {
        // Simulate some processing
        return await Task.FromResult($"Hello Command, {command.Message}!");
    }
}

public class HelloWorldCommand : ICommand<string>
{
    public string Message { get; set; }
}