
using System.Collections.Generic;
using NBL.Models;

namespace NBL.BLL.Contracts
{
    public interface IDepartmentManager:IManager<Department>
    {
        Department GetDepartmentByCode(string code);
        List<Designation> GetAllDesignationByDepartmentId(int departmentId);

    }
}
