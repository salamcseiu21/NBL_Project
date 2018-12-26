
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;

namespace NBL.Areas.Manager.Controllers
{
    [Authorize(Roles = "Distributor")]
    public class ManagerReportController : Controller
    {
        // GET: Manager/Report
       readonly  IOrderManager _iOrderManager;

        public ManagerReportController(IOrderManager iOrderManager)
        {
            _iOrderManager = iOrderManager;
        }
        public ActionResult SalesReport()  
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var sales = _iOrderManager.GetAll().ToList().FindAll(n => n.BranchId == branchId);
            return View(sales);
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string MGridHtmlExcel)
        {
            var fileName = Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 8) + ".xls";
            return File(Encoding.ASCII.GetBytes(MGridHtmlExcel), "application/vnd.ms-excel", fileName);
        }
      
    }
}