using System;
using NBL.Models;

namespace NblClassLibrary.Models
{
    public class User:Employee
    {
        public string UserName { set; get; }
        public string Password { get; set; }
        public int ActiveStaus { get; set; }
        public int BlockStatus { get; set; }
        public int UserRoleId { get; set; }
        public string Roles { get; set; }
        public string RoleName { get; set; }
        public int AddedByUserId { get; set; }
        public bool UserNameInUse { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public DateTime LogInDateTime { get; set; }
        public DateTime LogOutDateTime { get; set; }

        public string GetBasicInfo()
        {
            return EmployeeName;
        }
    }
}