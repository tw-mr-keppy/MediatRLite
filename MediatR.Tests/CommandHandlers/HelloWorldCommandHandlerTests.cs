using MediatR.Application;
using Shouldly;
using Xunit;

namespace MediatR.Tests.CommandHandlers;

public class HelloWorldCommandHandlerTests
{
    private readonly HelloWorldCommandHandler _handler;

    public HelloWorldCommandHandlerTests()
    {
        _handler = new HelloWorldCommandHandler();
    }

    [Fact]
    public async Task HandleAsync_WithMessage_ShouldReturnFormattedGreeting()
    {
        // Arrange
        var command = new HelloWorldCommand { Message = "Test" };
        
        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);
        
        // Assert
        result.ShouldBe("Hello Command, Test!");
    }
    
    [Fact]
    public async Task HandleAsync_WithEmptyMessage_ShouldReturnGreetingWithEmptyMessage()
    {
        // Arrange
        var command = new HelloWorldCommand { Message = string.Empty };
        
        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);
        
        // Assert
        result.ShouldBe("Hello Command, !");
    }
}