
using System.Collections.Generic;
using NBL.Models;

namespace NBL.BLL.Contracts
{
   public interface IDesignationManager
   {
       IEnumerable<Designation> GetAll();
        bool Save(Designation aDesignation);
        bool Update(Designation aDesignation);
        Designation GetDesignationByCode(string code);
        Designation GetDesignationById(int designationId);

    }
}
