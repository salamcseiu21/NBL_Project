﻿using System.Collections.Generic;
using System.Linq;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;

namespace NblClassLibrary.BLL
{
    public class EmployeeManager
    {

        readonly EmployeeGateway _employeeGateway=new EmployeeGateway();

        public IEnumerable<Employee> GetAll => _employeeGateway.GetAll;

        public Employee EmployeeById(int employeeId)
        {
            Employee employee= GetAll.ToList().Find(n => n.EmployeeId == employeeId);
            return employee;
        }

        public IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfo()
        {
           return _employeeGateway.GetAllEmployeeWithFullInfo();
        }
        public IEnumerable<ViewEmployee> GetAllEmployeeWithFullInfoByBranchId(int branchId)
        {
            return _employeeGateway.GetAllEmployeeWithFullInfoByBranchId(branchId);
        }
        public ViewEmployee GetEmployeeById(int empId)
        {
            ViewEmployee employee = _employeeGateway.GetEmployeeById(empId);
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
                    anEmployee.SubSubSubAccountCode = accountCode;
                    int rowAffected = _employeeGateway.Save(anEmployee);
                    if (rowAffected > 0)
                        return "Saved successfully!";
                    return "Failed to Save";
                }

                return "Email Address must be Unique";


            }

            return "Invalid Email Address";


            
        }
        private int GetEmployeeMaxSerialNo()
        {
            return _employeeGateway.GetEmployeeMaxSerialNo();
        }
        private bool IsEmailAddressUnique(string email)
        {
            Employee employee = _employeeGateway.GetEmployeeByEmailAddress(email); 
            return employee.EmployeeName == null;
        }

        private bool CheckEmail(string email)
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }

        public string Update(Employee anEmployee)
        {
            int rowAffected = _employeeGateway.Update(anEmployee); 
            if (rowAffected > 0)
                return "Updated successfully!";
            return "Failed to Update";
        }

        public IEnumerable<Employee> GetEmpoyeeListByDepartmentId(int departmentId)
        {
            return _employeeGateway.GetEmpoyeeListByDepartmentId(departmentId);
        }
    }
}