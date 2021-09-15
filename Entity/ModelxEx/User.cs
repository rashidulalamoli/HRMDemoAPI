using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Models
{
    public partial class User
    {
        [NotMapped]
        public string Password { get; set; }
    }
}
