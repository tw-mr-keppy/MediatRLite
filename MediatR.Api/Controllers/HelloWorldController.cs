using MediatR.Application;
using MediatRLite.Core;
using Microsoft.AspNetCore.Mvc;

namespace MediatR.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloWorldController : ControllerBase
{
    private readonly IMediator _mediator;

    public HelloWorldController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] HelloWorldCommand command)
    {
        var result = await _mediator.SendCommandAsync<HelloWorldCommand, string>(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string message)
    {
        var query = new HelloWorldQuery { Message = message };
        var result = await _mediator.SendQueryAsync<HelloWorldQuery, string>(query);
        return Ok(result);
    }
}