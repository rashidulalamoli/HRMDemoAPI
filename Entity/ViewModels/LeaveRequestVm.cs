using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class LeaveRequestVm
    {
        public string LeaveRequstGuid { get; set; }
        public int LeaveIssuer { get; set; }
        public int? LeaveApprover { get; set; }
        public string LeaveApplication { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public DateTime LeaveIssueDate { get; set; }
        public DateTime? LeaveApproveDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
