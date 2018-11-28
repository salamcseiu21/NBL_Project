using System.Collections.Generic;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;

namespace NblClassLibrary.BLL
{
    public class CompanyManager
    {

        readonly CompanyGateway _companyGateway = new CompanyGateway();

        public IEnumerable<Company> GetAll => _companyGateway.GetAll;

        public Company GetCompanyById(int companyId)
        {
            return _companyGateway.GetCompanyById(companyId);
        }
    }
}