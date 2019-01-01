﻿using System.Collections.Generic;
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
