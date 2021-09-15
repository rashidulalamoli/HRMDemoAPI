using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Notifications
{
    public class NotificationRegisterService : INotificationRegisterService
    {
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly IWebPushService _webPushService;
        private readonly INotificationPublisherService _notificationPublisherService;
        public NotificationRegisterService(IEmailService emailService, ISmsService smsService, IWebPushService webPushService, INotificationPublisherService notificationPublisherService)
        {
            _emailService = emailService;
            _smsService = smsService;
            _webPushService = webPushService;
            _notificationPublisherService = notificationPublisherService;
        }
        public void RegisterNotificationByPreference(List<int> subscriberIds, string msgBody)
        {
            // this register will be held dynamically by user.
            Parallel.ForEach(subscriberIds, id =>
            {
                switch (id)
                {
                    case 1:
                        _notificationPublisherService.EventChange += _emailService.onEventChange;
                        break;
                    case 2:
                        _notificationPublisherService.EventChange += _smsService.onEventChange;
                        break;
                    case 3:
                        _notificationPublisherService.EventChange += _webPushService.onEventChange;
                        break;
                    default:
                        break;
                }
            });
            //subscribe 
            _notificationPublisherService.Publish(msgBody);

        }

        public void RegisterNotification(string msgBody)
        {
            //register
            _notificationPublisherService.EventChange += _emailService.onEventChange;
            _notificationPublisherService.EventChange += _smsService.onEventChange;
            _notificationPublisherService.EventChange += _webPushService.onEventChange;
            //subscribe 
            _notificationPublisherService.Publish(msgBody);

        }

    }
}
