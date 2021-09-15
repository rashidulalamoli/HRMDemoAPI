using System;

namespace Utility.Notifications
{
    public class SmsService : ISmsService
    {
        public void onEventChange(object source, MessageEventArgs e)
        {
            System.Console.WriteLine(e.MessageBody);
        }
    }
}