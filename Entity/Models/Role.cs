using System.Collections.Generic;
using Entity.AuditModels;

namespace DataAccess.Models
{
    public partial class Role : DefaultAuditModel
    {
        public Role()
        {
            Users = new HashSet<User>();
        }
        public int RoleId { get; set; }
        public string RoleGuid { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
