using System.Collections.Generic;

namespace Utility.Notifications
{
    public interface INotificationRegisterService
    {
        void RegisterNotification(string msgBody);
        void RegisterNotificationByPreference(List<int> subscriberIds, string msgBody);
    }
}