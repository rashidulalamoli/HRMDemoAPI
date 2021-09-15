using Entity.AuditModels;

namespace DataAccess.Models
{
    public partial class NotificationManager : DefaultAuditModel
    {
        public int NotificationManagerId { get; set; }
        public string NotificationManagerGuid { get; set; }
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public Notification Notification { get; set; }
    }
}