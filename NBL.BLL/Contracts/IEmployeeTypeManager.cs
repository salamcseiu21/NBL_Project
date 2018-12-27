
using System.Collections.Generic;
using NBL.Models;

namespace NBL.BLL.Contracts
{
    public interface IEmployeeTypeManager
    {
        IEnumerable<EmployeeType> GetAll();
    }
}
