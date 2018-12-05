﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NblClassLibrary.Models
{
    public class User
    {
        public string UserName { set; get; }
        public string Password { get; set; }
        public int ActiveStaus { get; set; }
        public int BlockStatus { get; set; }
        public int UserRoleId { get; set; }
        public string Roles { get; set; }
        public int AddedByUserId { get; set; }
        public bool UserNameInUse { get; set; }
        public int EmployeeId { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Present Address")]
        public string PresentAddress { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Alternate Phone")]
        public string Email { get; set; }
        public DateTime JoiningDate { get; set; }
        [Display(Name = "Image")]
        public int UserId { get; set; }
        public Department Department { get; set; }
        public Designation Designation { get; set; }
     

    }
}