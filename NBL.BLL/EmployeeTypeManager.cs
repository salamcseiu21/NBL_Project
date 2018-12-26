using System.Collections.Generic;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;

namespace NBL.BLL
{
    public class EmployeeTypeManager:IEmployeeTypeManager
    {
        readonly EmployeeTypeGateway _employeeTypeGateway=new EmployeeTypeGateway();
        public IEnumerable<EmployeeType> GetAll => _employeeTypeGateway.GetAll; 
    }
}