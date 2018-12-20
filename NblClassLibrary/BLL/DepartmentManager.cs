using System.Collections.Generic;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;

namespace NblClassLibrary.BLL
{
    public class DepartmentManager
    {
        readonly  DepartmentGateway _departmentGateway=new DepartmentGateway();

        public IEnumerable<Department> GetAll => _departmentGateway.GetAll;

        public string Save(Department aDepartment)
        {
            int rowAffected=_departmentGateway.Save(aDepartment);
            if (rowAffected > 0)
                return "Department Save Successfully!";
            return "Failed to Save Department";
        }

        public string Update(Department aDepartment)
        {
            int rowAffected = _departmentGateway.Update(aDepartment);
            return rowAffected > 0 ? "Department information Updated Successfully!" : "Failed to Updated Department information";
        }

        public Department GetDepartmentByCode(string code)
        {
            return _departmentGateway.GetDepartmentByCode(code);
        }

        public Department GetDepartmentById(int deptId)
        {
            return _departmentGateway.GetDepartmentById(deptId);
        }

        public List<Designation> GetAllDesignationByDepartmentId(int departmentId)
        {
            return _departmentGateway.GetAllDesignationByDepartmentId(departmentId);
        }
    }
}