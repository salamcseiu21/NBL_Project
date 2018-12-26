using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;

namespace NBL.DAL.Contracts
{
    public interface IDesignationGateway
    {

        IEnumerable<Designation> GetAll();
        Designation GetDesignationByCode(string code);
        Designation GetDesignationById(int designationId);
        int Update(Designation aDesignation);
        int Save(Designation aDesignation);
        List<Designation> GetDesignationsByDepartmentId(int departmentId);


    }
}
