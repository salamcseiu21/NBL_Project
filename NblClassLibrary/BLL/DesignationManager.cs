using System.Collections.Generic;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;

namespace NblClassLibrary.BLL
{
    public class DesignationManager
    {
        readonly  DesignationGateway _designationGateway=new DesignationGateway();

        public IEnumerable<Designation> GetAll => _designationGateway.GetAll;

        public bool Save(Designation aDesignation)
        {
            int rowAffected = _designationGateway.Save(aDesignation);
            return rowAffected > 0;
        }

        public bool Update(Designation aDesignation)
        {
            int rowAffected = _designationGateway.Update(aDesignation);
            return rowAffected > 0;
               
        }

        public Designation GetDesignationByCode(string code)
        {
            return _designationGateway.GetDesignationByCode(code);
        }
        public Designation GetDesignationById(int designationId)
        {
            return _designationGateway.GetDesignationById(designationId);
        }
    }
}