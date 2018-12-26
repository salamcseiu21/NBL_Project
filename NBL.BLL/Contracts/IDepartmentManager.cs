using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;

namespace NBL.BLL.Contracts
{
    public interface IDepartmentManager
    {
        IEnumerable<Department> GetAll();

        string Save(Department aDepartment);
        

        string Update(Department aDepartment);
        

        Department GetDepartmentByCode(string code);
        

        Department GetDepartmentById(int deptId);
       

        List<Designation> GetAllDesignationByDepartmentId(int departmentId);

    }
}
