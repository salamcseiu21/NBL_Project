using System.Collections.Generic;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;

namespace NblClassLibrary.BLL
{
    public class EmployeeTypeManager
    {
        readonly EmployeeTypeGateway _employeeTypeGateway=new EmployeeTypeGateway();
        public IEnumerable<EmployeeType> GetAll => _employeeTypeGateway.GetAll; 
    }
}