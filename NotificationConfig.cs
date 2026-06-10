public class NotificationConfig
{
    public List<NotificationTemplate> Templates { get; set; } = [];
}

public class NotificationTemplate
{
    public required string Event { get; set; }
    public required string Message { get; set; }
}