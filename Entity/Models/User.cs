using Entity.AuditModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public partial class User : DefaultAuditModel
    {
        public int UserId { get; set; }
        public string UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string Image { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
