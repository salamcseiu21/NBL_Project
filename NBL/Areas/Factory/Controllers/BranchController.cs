
using System.Linq;
using System.Web.Mvc;
using NBL.BLL;

namespace NBL.Areas.Factory.Controllers
{
    [Authorize(Roles = "Factory")]
    public class BranchController : Controller
    {
        // GET: Factory/Branch
        readonly BranchManager _branchManager = new BranchManager();
        [HttpPost] 

        public JsonResult BranchAutoComplete(string prefix)
        {
            int corporateBarachIndex = _branchManager.GetAll().ToList().FindIndex(n => n.BranchName.Contains("Corporate"));
            var branches = _branchManager.GetAll().ToList();
            branches.RemoveAt(corporateBarachIndex);
            var branchList = (from c in branches.ToList()
                where c.BranchName.ToLower().Contains(prefix.ToLower())
                select new
                {
                    label = c.BranchName,
                    val = c.BranchId
                }).ToList();

            return Json(branchList);
        }

        public JsonResult GetBranchDetailsById(int branchId)
        {
          var branch=_branchManager.GetBranchById(branchId);
          return Json(branch, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewBranch()
        {
            var branches = _branchManager.GetAll().ToList();
            return View(branches);

        }
    }
}