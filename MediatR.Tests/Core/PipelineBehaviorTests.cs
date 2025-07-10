using MediatRLite.Core.Behaviors;
using MediatRLite.Core.Commands;
using NSubstitute;
using Shouldly;
using Xunit;

namespace MediatR.Tests.Core;

public class PipelineBehaviorTests
{
    [Fact]
    public async Task HandleAsync_WithCommandResult_ShouldExecuteBehaviorAndCallNext()
    {
        // Arrange
        var command = new TestCommand();
        var expectedResult = "Test Result";
        
        var behavior = new TestPipelineBehavior();
        Func<Task<string>> next = () => Task.FromResult(expectedResult);
        
        // Act
        var result = await behavior.HandleAsync(command, next, CancellationToken.None);
        
        // Assert
        result.ShouldBe(expectedResult);
        behavior.CommandHandled.ShouldBe(command);
        behavior.NextCalled.ShouldBeTrue();
    }
    
    [Fact]
    public async Task HandleAsync_WithVoidCommand_ShouldExecuteBehaviorAndCallNext()
    {
        // Arrange
        var command = new TestVoidCommand();
        var behavior = new TestVoidPipelineBehavior();
        var nextCalled = false;
        
        Func<Task> next = () => 
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        
        // Act
        await behavior.HandleAsync(command, next, CancellationToken.None);
        
        // Assert
        behavior.CommandHandled.ShouldBe(command);
        nextCalled.ShouldBeTrue();
    }
    
    [Fact]
    public async Task HandleAsync_WithMultipleBehaviors_ShouldExecuteInCorrectOrder()
    {
        // Arrange
        var command = new TestCommand();
        var expectedResult = "Test Result";
        var executionOrder = new List<string>();
        
        var behavior1 = new OrderedPipelineBehavior("First", executionOrder);
        var behavior2 = new OrderedPipelineBehavior("Second", executionOrder);
        
        Func<Task<string>> originalNext = () => 
        {
            executionOrder.Add("Handler");
            return Task.FromResult(expectedResult);
        };
        
        Func<Task<string>> next2 = () => behavior1.HandleAsync(command, originalNext, CancellationToken.None);
        
        // Act
        var result = await behavior2.HandleAsync(command, next2, CancellationToken.None);
        
        // Assert
        result.ShouldBe(expectedResult);
        executionOrder.ShouldBe(new[] { "Second.Before", "First.Before", "Handler", "First.After", "Second.After" });
    }
    
    // Test classes
    public class TestCommand : ICommand<string> { }
    
    public class TestVoidCommand : ICommand { }
    
    public class TestPipelineBehavior : IPipelineBehavior<TestCommand, string>
    {
        public TestCommand? CommandHandled { get; private set; }
        public bool NextCalled { get; private set; }
        
        public async Task<string> HandleAsync(TestCommand request, Func<Task<string>> next, CancellationToken cancellationToken)
        {
            CommandHandled = request;
            var result = await next();
            NextCalled = true;
            return result;
        }
    }
    
    public class TestVoidPipelineBehavior : IPipelineBehavior<TestVoidCommand>
    {
        public TestVoidCommand? CommandHandled { get; private set; }
        
        public async Task HandleAsync(TestVoidCommand request, Func<Task> next, CancellationToken cancellationToken)
        {
            CommandHandled = request;
            await next();
        }
    }
    
    public class OrderedPipelineBehavior : IPipelineBehavior<TestCommand, string>
    {
        private readonly string _name;
        private readonly List<string> _executionOrder;
        
        public OrderedPipelineBehavior(string name, List<string> executionOrder)
        {
            _name = name;
            _executionOrder = executionOrder;
        }
        
        public async Task<string> HandleAsync(TestCommand request, Func<Task<string>> next, CancellationToken cancellationToken)
        {
            _executionOrder.Add($"{_name}.Before");
            var result = await next();
            _executionOrder.Add($"{_name}.After");
            return result;
        }
    }
}