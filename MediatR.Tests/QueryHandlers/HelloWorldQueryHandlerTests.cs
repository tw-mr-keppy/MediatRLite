using MediatR.Application;
using Shouldly;
using Xunit;

namespace MediatR.Tests.QueryHandlers;

public class HelloWorldQueryHandlerTests
{
    private readonly HelloWorldQueryHandler _handler;

    public HelloWorldQueryHandlerTests()
    {
        _handler = new HelloWorldQueryHandler();
    }

    [Fact]
    public async Task HandleAsync_WithMessage_ShouldReturnFormattedGreeting()
    {
        // Arrange
        var query = new HelloWorldQuery { Message = "Test" };
        
        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);
        
        // Assert
        result.ShouldBe("Hello Query, Test!");
    }
    
    [Fact]
    public async Task HandleAsync_WithEmptyMessage_ShouldReturnGreetingWithEmptyMessage()
    {
        // Arrange
        var query = new HelloWorldQuery { Message = string.Empty };
        
        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);
        
        // Assert
        result.ShouldBe("Hello Query, !");
    }
}