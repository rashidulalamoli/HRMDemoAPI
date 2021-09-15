using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class UserVm
    {
        public string UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public int RoleId { get; set; }
    }
}
