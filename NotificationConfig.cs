public class NotificationConfig
{
    public List<NotificationTemplate> Templates { get; set; }
}

public class NotificationTemplate
{
    public string Event { get; set; }

    public string Message { get; set; }
}