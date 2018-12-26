using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;

namespace NBL.DAL.Contracts
{
    public interface IDepartmentGateway
    {
         IEnumerable<Department> GetAll();

        int Update(Department aDepartment);
        int Save(Department aDepartment);
      
        List<Designation> GetAllDesignationByDepartmentId(int departmentId);
        

        Department GetDepartmentByCode(string code);
        
        Department GetDepartmentById(int deptId);

    }
}
