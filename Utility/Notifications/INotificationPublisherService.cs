namespace Utility.Notifications
{
    public interface INotificationPublisherService
    {
        event NotificationPublisherService.NotificationServiceEventHandler EventChange;

        void Publish(string body);
    }
}