using MediatRLite.Core.Queries;

namespace MediatR.Application;

public class HelloWorldQueryHandler : IQueryHandler<HelloWorldQuery, string>
{
    public async Task<string> HandleAsync(HelloWorldQuery query, CancellationToken cancellationToken = default)
    {
        // Simulate some processing
        return await Task.FromResult($"Hello Query, {query.Message}!");
    }
}

public class HelloWorldQuery : IQuery<string>
{
    public string Message { get; set; }
}