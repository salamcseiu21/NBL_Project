using System.Collections.Generic;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;

namespace NBL.BLL
{
    public class DepartmentManager:IDepartmentManager
    {
        readonly IDepartmentGateway _iDepartmentGateway;

        public DepartmentManager(IDepartmentGateway iDepartmentGateway)
        {
            _iDepartmentGateway = iDepartmentGateway;
        }
        public IEnumerable<Department> GetAll()
        {
            return _iDepartmentGateway.GetAll();
        } 

        public string Save(Department aDepartment)
        {
            int rowAffected= _iDepartmentGateway.Save(aDepartment);
            if (rowAffected > 0)
                return "Department Save Successfully!";
            return "Failed to Save Department";
        }

        public string Update(Department aDepartment)
        {
            int rowAffected = _iDepartmentGateway.Update(aDepartment);
            return rowAffected > 0 ? "Department information Updated Successfully!" : "Failed to Updated Department information";
        }

        public Department GetDepartmentByCode(string code)
        {
            return _iDepartmentGateway.GetDepartmentByCode(code);
        }
        public Department GetDepartmentById(int deptId)
        {
            return _iDepartmentGateway.GetDepartmentById(deptId);
        }

        public List<Designation> GetAllDesignationByDepartmentId(int departmentId)
        {
            return _iDepartmentGateway.GetAllDesignationByDepartmentId(departmentId);
        }
    }
}