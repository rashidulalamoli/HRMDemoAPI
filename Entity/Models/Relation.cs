using System;
using System.Collections.Generic;
using System.Text;
using Entity.AuditModels;

namespace DataAccess.Models
{
    public partial class Relation : DefaultAuditModel
    {
        public int RelationId { get; set; }
        public string RelationGuid { get; set; }
        public string Name { get; set; }
    }
}
