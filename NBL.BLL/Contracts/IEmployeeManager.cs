using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
   public interface IEmployeeManager
    {

         IEnumerable<Employee> GetAll();
        Employee EmployeeById(int employeeId);
       
        IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfo();
     
        IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfoByBranchId(int branchId);

        ViewEmployee GetEmployeeById(int empId);
       
        string Save(Employee anEmployee);
  

        int GetEmployeeMaxSerialNo();
        
        bool IsEmailAddressUnique(string email);
     

        bool CheckEmail(string email);

        string Update(Employee anEmployee);
       
        IEnumerable<Employee> GetEmpoyeeListByDepartmentId(int departmentId);

    }
}
