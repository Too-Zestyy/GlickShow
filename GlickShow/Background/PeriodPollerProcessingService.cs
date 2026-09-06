using Microsoft.EntityFrameworkCore;

internal interface IScopedProcessingService
{
    Task DoWork(CancellationToken stoppingToken);
}

internal class PeriodPollerProcessingService : IScopedProcessingService
{
    private int executionCount = 0;
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _service;
    
    public PeriodPollerProcessingService(ILogger<PeriodPollerProcessingService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _service = scopeFactory;
    }

    public async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            executionCount++;

            using var scope = _service.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<GlickoContext>();

            var system = await context.Systems.OrderByDescending(s => s.ID).FirstAsync();

            _logger.LogInformation(
                "Scoped Processing Service is working. Count: {Count}", executionCount);
            _logger.LogInformation(
                "Scoped processing Service got last system ID: {}", system.ID
            );

            await Task.Delay(10000, stoppingToken);
        }
    }
}