using NblClassLibrary.Models;
using NBL.Models;
namespace NBL
{
    public class ViewEmployee:Employee
    {
        public string EmployeeTypeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string BranchName { get; set; } 
    }
}