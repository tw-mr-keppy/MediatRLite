using MediatRLite.Core;
using MediatRLite.Core.Behaviors;
using MediatRLite.Core.Commands;
using MediatRLite.Core.Queries;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Xunit;

namespace MediatR.Tests.Core;

public class MediatorTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Mediator _mediator;
    
    public MediatorTests()
    {
        _serviceProvider = Substitute.For<IServiceProvider>();
        
        // Register empty collections of behaviors for all test types
        _serviceProvider.GetService(typeof(IEnumerable<IPipelineBehavior<TestCommand, string>>))
            .Returns(Array.Empty<IPipelineBehavior<TestCommand, string>>());
        
        _serviceProvider.GetService(typeof(IEnumerable<IPipelineBehavior<TestVoidCommand>>))
            .Returns(Array.Empty<IPipelineBehavior<TestVoidCommand>>());
        
        _serviceProvider.GetService(typeof(IEnumerable<IPipelineBehavior<TestQuery, string>>))
            .Returns(Array.Empty<IPipelineBehavior<TestQuery, string>>());
        
        _mediator = new Mediator(_serviceProvider);
    }
    
    [Fact]
    public async Task SendCommandAsync_WithResult_ShouldCallHandlerAndReturnResult()
    {
        // Arrange
        var command = new TestCommand();
        var handler = Substitute.For<ICommandHandler<TestCommand, string>>();
        var expectedResult = "Test Result";
        
        handler.HandleAsync(command, Arg.Any<CancellationToken>()).Returns(expectedResult);
        _serviceProvider.GetService(typeof(ICommandHandler<TestCommand, string>)).Returns(handler);
        
        // Act
        var result = await _mediator.SendCommandAsync<TestCommand, string>(command);
        
        // Assert
        result.ShouldBe(expectedResult);
        await handler.Received(1).HandleAsync(command, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task SendCommandAsync_Void_ShouldCallHandler()
    {
        // Arrange
        var command = new TestVoidCommand();
        var handler = Substitute.For<ICommandHandler<TestVoidCommand>>();
        
        _serviceProvider.GetService(typeof(ICommandHandler<TestVoidCommand>)).Returns(handler);
        
        // Act
        await _mediator.SendCommandAsync(command);
        
        // Assert
        await handler.Received(1).HandleAsync(command, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task SendQueryAsync_ShouldCallHandlerAndReturnResult()
    {
        // Arrange
        var query = new TestQuery();
        var handler = Substitute.For<IQueryHandler<TestQuery, string>>();
        var expectedResult = "Test Result";
        
        handler.HandleAsync(query, Arg.Any<CancellationToken>()).Returns(expectedResult);
        _serviceProvider.GetService(typeof(IQueryHandler<TestQuery, string>)).Returns(handler);
        
        // Act
        var result = await _mediator.SendQueryAsync<TestQuery, string>(query);
        
        // Assert
        result.ShouldBe(expectedResult);
        await handler.Received(1).HandleAsync(query, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public void SendCommandAsync_WithResult_ShouldThrowWhenHandlerNotRegistered()
    {
        // Arrange
        var command = new TestCommand();
        _serviceProvider.GetService(typeof(ICommandHandler<TestCommand, string>)).Returns(null);
        
        // Act & Assert
        Should.Throw<InvalidOperationException>(async () => 
            await _mediator.SendCommandAsync<TestCommand, string>(command));
    }
    
    [Fact]
    public void SendCommandAsync_Void_ShouldThrowWhenHandlerNotRegistered()
    {
        // Arrange
        var command = new TestVoidCommand();
        _serviceProvider.GetService(typeof(ICommandHandler<TestVoidCommand>)).Returns(null);
        
        // Act & Assert
        Should.Throw<InvalidOperationException>(async () => 
            await _mediator.SendCommandAsync(command));
    }
    
    [Fact]
    public void SendQueryAsync_ShouldThrowWhenHandlerNotRegistered()
    {
        // Arrange
        var query = new TestQuery();
        _serviceProvider.GetService(typeof(IQueryHandler<TestQuery, string>)).Returns(null);
        
        // Act & Assert
        Should.Throw<InvalidOperationException>(async () => 
            await _mediator.SendQueryAsync<TestQuery, string>(query));
    }
    
    // Test classes
    public class TestCommand : ICommand<string> { }
    
    public class TestVoidCommand : ICommand { }
    
    public class TestQuery : IQuery<string> { }
}