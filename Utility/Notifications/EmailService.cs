using System;

namespace Utility.Notifications
{
    public class EmailService : IEmailService
    {
        public void onEventChange(object source, MessageEventArgs e)
        {
            System.Console.WriteLine(e.MessageBody);
        }
    }
}