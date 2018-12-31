
using System;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Factory.Controllers
{
    [Authorize(Roles ="Factory")]
    public class ApproveController : Controller
    {

        private readonly IProductManager _iProductManager;
        // GET: Factory/Approve

        public ApproveController(IProductManager iProductManager)
        {
            _iProductManager = iProductManager;
        }
        public ActionResult PendingTransferIssueList()
        {
            var issuedProducts = _iProductManager.GetTransferIssueList();
            return View(issuedProducts);
        }
        public ActionResult ApproveTransferIssue(int id)
        {
            ViewBag.Transfer = _iProductManager.GetTransferIssueList().ToList().Find(n => n.TransferIssueId == id);
            var issueDetails = _iProductManager.GetTransferIssueDetailsById(id);
            return View(issueDetails);
        }
        [HttpPost]
        public ActionResult ApproveTransferIssue(FormCollection collection,int id)
        {
            int transferIssueId = Convert.ToInt32(collection["TransferIssueId"]);
            int approveUserId = ((ViewUser)Session["user"]).UserId;
            DateTime approveDateTime = DateTime.Now;
            TransferIssue transferIssue = new TransferIssue
            {
                ApproveByUserId = approveUserId,
                TransferIssueId = transferIssueId,
                ApproveDateTime = approveDateTime
            };
            bool result = _iProductManager.ApproveTransferIssue(transferIssue);
            if (result)
            {
                return RedirectToAction("PendingTransferIssueList");
            }
            ViewBag.Transfer = _iProductManager.GetTransferIssueList().ToList().Find(n => n.TransferIssueId == transferIssueId);
            var issueDetails = _iProductManager.GetTransferIssueDetailsById(transferIssueId);
            return View(issueDetails);
        }

    }
}