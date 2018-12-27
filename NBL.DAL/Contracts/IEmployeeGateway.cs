using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
   public interface IEmployeeGateway:IGateway<Employee>
   {
       IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfo();
       IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfoByBranchId(int branchId);
       ViewEmployee GetEmployeeById(int empId);
       Employee GetEmployeeByEmailAddress(string email);
       int GetEmployeeMaxSerialNo();
       IEnumerable<Employee> GetEmpoyeeListByDepartmentId(int departmentId);
   }
}
