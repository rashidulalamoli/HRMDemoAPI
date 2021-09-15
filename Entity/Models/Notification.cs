using Entity.AuditModels;

namespace DataAccess.Models
{
    public partial class Notification : DefaultAuditModel
    {
        public int NotificationId { get; set; }
        public string NotificationGuid { get; set; }
        public string Name { get; set; }
    }
}