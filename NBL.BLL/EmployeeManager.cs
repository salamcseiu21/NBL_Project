using System;
using System.Collections.Generic;
using AutoMapper;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL
{
    public class EmployeeManager:IEmployeeManager
    {

        private readonly IEmployeeGateway _iEmployeeGateway;

        public EmployeeManager(IEmployeeGateway iEmployeeGateway)
        {
            _iEmployeeGateway = iEmployeeGateway;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _iEmployeeGateway.GetAll();
        }

        public Employee EmployeeById(int employeeId)
        {
            var viewEmployee = _iEmployeeGateway.GetEmployeeById(employeeId);
            var employee=Mapper.Map<Employee>(viewEmployee);
            return employee;
        }

        public IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfo()
        {
           return _iEmployeeGateway.GetAllEmployeeWithFullInfo();
        }
        public IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfoByBranchId(int branchId)
        {
            return _iEmployeeGateway.GetAllEmployeeWithFullInfoByBranchId(branchId);
        }
        public ViewEmployee GetEmployeeById(int empId)
        {
            ViewEmployee employee = _iEmployeeGateway.GetEmployeeById(empId);
            return employee;
        }

        public string Save(Employee anEmployee)
        {

            bool checkEmail = CheckEmail(anEmployee.Email);
            if (checkEmail)
            {

                bool isUnique = IsEmailAddressUnique(anEmployee.Email);
                if (isUnique)
                {
                    int lastSlN=GetEmployeeMaxSerialNo();
                    string accountCode =Generator.GenerateAccountCode("3301", lastSlN);
                    anEmployee.EmployeeNo = Generator.GenerateEmployeeNo(anEmployee,lastSlN);
                    anEmployee.SubSubSubAccountCode = accountCode;
                    int rowAffected = _iEmployeeGateway.Save(anEmployee);
                    if (rowAffected > 0)
                        return "Saved successfully!";
                    return "Failed to Save";
                }

                return "Email Address must be Unique";


            }

            return "Invalid Email Address";


            
        }
        public int GetEmployeeMaxSerialNo()
        {
            return _iEmployeeGateway.GetEmployeeMaxSerialNo();
        }
        public bool IsEmailAddressUnique(string email)
        {
            Employee employee = _iEmployeeGateway.GetEmployeeByEmailAddress(email); 
            return employee.EmployeeName == null;
        }

        public bool CheckEmail(string email)
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }

        public string Update(Employee anEmployee)
        {
            var empNo=Convert.ToInt32(anEmployee.EmployeeNo.Substring(anEmployee.EmployeeNo.Length - 3, 3))-1;
            anEmployee.EmployeeNo = Generator.GenerateEmployeeNo(anEmployee, empNo);
            int rowAffected = _iEmployeeGateway.Update(anEmployee);
            if (rowAffected > 0)
                return "Updated successfully!";
            return "Failed to Update";
        }

        public IEnumerable<Employee> GetEmpoyeeListByDepartmentId(int departmentId)
        {
            return _iEmployeeGateway.GetEmpoyeeListByDepartmentId(departmentId);
        }
    }
}