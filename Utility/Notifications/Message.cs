using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Notifications
{
    public class MessageEventArgs: EventArgs
    {
        public string MessageBody { get; set; }
    }
}
