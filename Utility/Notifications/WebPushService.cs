using System;

namespace Utility.Notifications
{
    public class WebPushService : IWebPushService
    {
        public void onEventChange(object source, MessageEventArgs e)
        {
            System.Console.WriteLine(e.MessageBody);
        }
    }
}