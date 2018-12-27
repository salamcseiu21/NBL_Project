using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;

namespace NBL.BLL.Contracts
{
    public interface IDepartmentManager:IManager<Department>
    {
       

        Department GetDepartmentByCode(string code);
        List<Designation> GetAllDesignationByDepartmentId(int departmentId);

    }
}
