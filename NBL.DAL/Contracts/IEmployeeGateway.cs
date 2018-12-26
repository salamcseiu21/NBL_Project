using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
   public interface IEmployeeGateway
   {
       IEnumerable<Employee> GetAll();
       IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfo();
       IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfoByBranchId(int branchId);
       ViewEmployee GetEmployeeById(int empId);
       int Save(Employee anEmployee);
       Employee GetEmployeeByEmailAddress(string email);
       int GetEmployeeMaxSerialNo();
       int Update(Employee anEmployee);
       IEnumerable<Employee> GetEmpoyeeListByDepartmentId(int departmentId);
   }
}
