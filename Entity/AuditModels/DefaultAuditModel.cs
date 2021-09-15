using System;

namespace Entity.AuditModels
{
    public class DefaultAuditModel
    {
        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
