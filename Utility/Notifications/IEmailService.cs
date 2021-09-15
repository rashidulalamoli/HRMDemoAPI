using System;

namespace Utility.Notifications
{
    public interface IEmailService
    {
        void onEventChange(object source, MessageEventArgs e);
    }
}