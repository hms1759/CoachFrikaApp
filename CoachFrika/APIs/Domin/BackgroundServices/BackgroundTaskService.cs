namespace CoachFrika.APIs.Domin.BackgroundServices
{
    public class BackgroundTaskService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<BackgroundTaskService> _logger;

        public BackgroundTaskService(IBackgroundTaskQueue taskQueue, ILogger<BackgroundTaskService> logger)
        {
            _taskQueue = taskQueue ?? throw new ArgumentNullException(nameof(taskQueue));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background task service is starting.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var workItem = await _taskQueue.DequeueAsync(stoppingToken);

                    try
                    {
                        await workItem(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred executing background task.");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Background task service is stopping.");
            }
        }
    }
}
