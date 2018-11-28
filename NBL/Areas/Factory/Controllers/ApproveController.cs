
using System;
using System.Linq;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;

namespace NBL.Areas.Factory.Controllers
{
    [Authorize(Roles ="Factory")]
    public class ApproveController : Controller
    {

        readonly ProductManager _productManager = new ProductManager();
        // GET: Factory/Approve


        public ActionResult PendingTransferIssueList()
        {
            var issuedProducts = _productManager.GetTransferIssueList();
            return View(issuedProducts);
        }
        public ActionResult ApproveTransferIssue(int id)
        {
            ViewBag.Transfer = _productManager.GetTransferIssueList().ToList().Find(n => n.TransferIssueId == id);
            var issueDetails = _productManager.GetTransferIssueDetailsById(id);
            return View(issueDetails);
        }
        [HttpPost]
        public ActionResult ApproveTransferIssue(FormCollection collection,int id)
        {
            int transferIssueId = Convert.ToInt32(collection["TransferIssueId"]);
            int approveUserId = ((User)Session["user"]).UserId;
            DateTime approveDateTime = DateTime.Now;
            TransferIssue transferIssue = new TransferIssue
            {
                ApproveByUserId = approveUserId,
                TransferIssueId = transferIssueId,
                ApproveDateTime = approveDateTime
            };
            bool result = _productManager.ApproveTransferIssue(transferIssue);
            if (result)
            {
                return RedirectToAction("PendingTransferIssueList");
            }
            ViewBag.Transfer = _productManager.GetTransferIssueList().ToList().Find(n => n.TransferIssueId == transferIssueId);
            var issueDetails = _productManager.GetTransferIssueDetailsById(transferIssueId);
            return View(issueDetails);
        }

    }
}