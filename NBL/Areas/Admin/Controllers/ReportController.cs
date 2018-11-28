using System.Web.Mvc;

namespace NBL.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        // GET: Admin/Report
        public ActionResult Index()
        {
            return View();
        }
    }
}