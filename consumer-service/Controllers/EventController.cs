using Microsoft.AspNetCore.Mvc;

namespace consumer_service.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    private readonly ILogger<EventController> _logger;

    public EventController(ILogger<EventController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Event @event)
    {
#if DEBUG // this following line will not be compiled into the code when it is compiled for release
        await System.IO.File.WriteAllTextAsync("/app/output.txt", $"{@event.Value} - {@event.DateTimeOffset}");
#endif
        _logger.LogInformation("Received event successfully. Value: {Value}, Time: {Time}", @event.Value, @event.DateTimeOffset);
        return Ok();
    }
}

public class Event
{
    public required string Value { get; set; }
    public DateTimeOffset DateTimeOffset { get; set; }
}
