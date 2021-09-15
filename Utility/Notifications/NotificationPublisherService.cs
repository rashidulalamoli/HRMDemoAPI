using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Notifications
{
    public class NotificationPublisherService : INotificationPublisherService
    {
        public delegate void NotificationServiceEventHandler(object source, MessageEventArgs e);
        public event NotificationServiceEventHandler EventChange;

        public void Publish(string msg)
        {
            OnEventChange(msg);
        }
        protected virtual void OnEventChange(string msg)
        {
            if (EventChange != null)
            {
                EventChange(this, new MessageEventArgs() { MessageBody = msg});
            }
        }
    }
}
