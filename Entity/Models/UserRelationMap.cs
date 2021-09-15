using Entity.AuditModels;

namespace DataAccess.Models
{
    public partial class UserRelationMap : DefaultAuditModel
    {
        public int UserRelationMapId { get; set; }
        public int UserRelationMapGuid { get; set; }
        public int RelationId { get; set; }
        public int UserId { get; set; }
        public int SupervisorId { get; set; }
    }
}
