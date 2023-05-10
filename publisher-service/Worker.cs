using Refit;

namespace publisher_service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IEventClient _eventClient;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _eventClient = RestService.For<IEventClient>("http://consumer-service");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);

                await _eventClient.SendEvent(new()
                {
                    Value = $"Hello from the other side",
                    DateTimeOffset = DateTimeOffset.UtcNow
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occured while sending request");
            }
        }
    }
}

public interface IEventClient
{
    [Post("/event")]
    Task SendEvent([Body] Event @event);
}

public class Event
{
    public required string Value { get; set; }
    public DateTimeOffset DateTimeOffset { get; set; }
}
