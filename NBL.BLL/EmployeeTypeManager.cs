using System.Collections.Generic;
using NBL.DAL;
using NBL.Models;

namespace NBL.BLL
{
    public class EmployeeTypeManager
    {
        readonly EmployeeTypeGateway _employeeTypeGateway=new EmployeeTypeGateway();
        public IEnumerable<EmployeeType> GetAll => _employeeTypeGateway.GetAll; 
    }
}