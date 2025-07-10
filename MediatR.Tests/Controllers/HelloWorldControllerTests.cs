using MediatR.Api.Controllers;
using MediatR.Application;
using MediatRLite.Core;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace MediatR.Tests.Controllers;

public class HelloWorldControllerTests
{
    private readonly IMediator _mediator;
    private readonly HelloWorldController _controller;

    public HelloWorldControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new HelloWorldController(_mediator);
    }

    [Fact]
    public async Task Post_WithCommand_ShouldReturnOkResultWithCommandResult()
    {
        // Arrange
        var command = new HelloWorldCommand { Message = "Test" };
        var expectedResult = "Hello Command, Test!";
        
        _mediator.SendCommandAsync<HelloWorldCommand, string>(command)
            .Returns(expectedResult);
        
        // Act
        var result = await _controller.Post(command);
        
        // Assert
        var okResult = result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBe(expectedResult);
        
        await _mediator.Received(1).SendCommandAsync<HelloWorldCommand, string>(
            Arg.Is<HelloWorldCommand>(c => c.Message == command.Message));
    }

    [Fact]
    public async Task Get_WithMessage_ShouldReturnOkResultWithQueryResult()
    {
        // Arrange
        var message = "Test";
        var expectedResult = "Hello Query, Test!";
        
        _mediator.SendQueryAsync<HelloWorldQuery, string>(
            Arg.Is<HelloWorldQuery>(q => q.Message == message))
            .Returns(expectedResult);
        
        // Act
        var result = await _controller.Get(message);
        
        // Assert
        var okResult = result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBe(expectedResult);
        
        await _mediator.Received(1).SendQueryAsync<HelloWorldQuery, string>(
            Arg.Is<HelloWorldQuery>(q => q.Message == message));
    }
}