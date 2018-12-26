using System.Collections.Generic;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;

namespace NBL.BLL
{
    public class CompanyManager:ICompanyManager
    {
        readonly ICompanyGateway _iCompanyGateway;

        public CompanyManager(ICompanyGateway iCompanyGateway)
        {
            _iCompanyGateway = iCompanyGateway;
        }
       

        public IEnumerable<Company> GetAll()
        {
            return _iCompanyGateway.GetAll();
        }

        public Company GetCompanyById(int companyId)
        {
            return _iCompanyGateway.GetCompanyById(companyId);
        }
    }
}