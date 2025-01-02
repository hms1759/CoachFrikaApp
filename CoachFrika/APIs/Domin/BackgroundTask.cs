namespace CoachFrika.APIs.Domin
{
    public class BackgroundTask
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; } = string.Empty;
        public Func<CancellationToken, Task> WorkItem { get; set; } = default!;
    }

}
