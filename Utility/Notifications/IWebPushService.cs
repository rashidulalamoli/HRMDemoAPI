using System;

namespace Utility.Notifications
{
    public interface IWebPushService
    {
        void onEventChange(object source, MessageEventArgs e);
    }
}