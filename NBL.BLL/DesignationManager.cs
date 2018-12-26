using System.Collections.Generic;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;

namespace NBL.BLL
{
    public class DesignationManager:IDesignationManager
    {
        readonly  IDesignationGateway _iDesignationGateway;

        public DesignationManager(IDesignationGateway iDesignationGateway)
        {
            _iDesignationGateway = iDesignationGateway;
        }

        public IEnumerable<Designation> GetAll()
        {
            return  _iDesignationGateway.GetAll();
        }

        public bool Save(Designation aDesignation)
        {
            int rowAffected = _iDesignationGateway.Save(aDesignation);
            return rowAffected > 0;
        }

        public bool Update(Designation aDesignation)
        {
            int rowAffected = _iDesignationGateway.Update(aDesignation);
            return rowAffected > 0;  
        }

        public Designation GetDesignationByCode(string code)
        {
            return _iDesignationGateway.GetDesignationByCode(code);
        }
        public Designation GetDesignationById(int designationId)
        {
            return _iDesignationGateway.GetDesignationById(designationId);
        }
    }
}