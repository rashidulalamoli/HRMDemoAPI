using System;

namespace Utility.Notifications
{
    public interface ISmsService
    {
        void onEventChange(object source, MessageEventArgs e);
    }
}