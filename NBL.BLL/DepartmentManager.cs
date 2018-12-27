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

        public Department GetById(int id)
        {
            return _iDepartmentGateway.GetById(id);
        }

        public ICollection<Department> GetAll()
        {
            return _iDepartmentGateway.GetAll();
        } 

        public bool Add(Department model)
        {
            return _iDepartmentGateway.Add(model)>0;
            
        }
        

        public bool Delete(Department model)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Department aDepartment)
        {

            return  _iDepartmentGateway.Update(aDepartment)>0 ;
        }

        public Department GetDepartmentByCode(string code)
        {
            return _iDepartmentGateway.GetDepartmentByCode(code);
        }
     

        public List<Designation> GetAllDesignationByDepartmentId(int departmentId)
        {
            return _iDepartmentGateway.GetAllDesignationByDepartmentId(departmentId);
        }
    }
}