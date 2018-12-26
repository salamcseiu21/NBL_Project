using System.Collections.Generic;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.Models;

namespace NBL.BLL
{
    public class CompanyManager:ICompanyManager
    {

        readonly CompanyGateway _companyGateway = new CompanyGateway();

        public IEnumerable<Company> GetAll => _companyGateway.GetAll;

        public Company GetCompanyById(int companyId)
        {
            return _companyGateway.GetCompanyById(companyId);
        }
    }
}