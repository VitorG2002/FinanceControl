namespace NotificationService
{
    public interface INotificationEmailService
    {
        Task ProcessMessageAsync(string message);
    }
}
