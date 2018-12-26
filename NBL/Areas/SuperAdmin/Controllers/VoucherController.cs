using NBL.Areas.Accounts.BLL;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NBL.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "Super")]
    public class VoucherController : Controller
    {
       private readonly AccountsManager _accountsManager = new AccountsManager();
        // GET: SuperAdmin/Voucher
        public PartialViewResult ViewJournal()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var journals = _accountsManager.GetAllJournalVouchersByCompanyId(companyId).ToList();
            return PartialView("_ViewJournalPartialPage",journals);
        }
    }
}