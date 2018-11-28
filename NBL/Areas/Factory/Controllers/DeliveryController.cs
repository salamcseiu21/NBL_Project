
using NBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;
using NBL.Areas.Factory.BLL;
namespace NBL.Areas.Factory.Controllers
{
    [Authorize(Roles ="Factory")]
    public class DeliveryController : Controller
    {

        readonly ProductManager _productManager = new ProductManager();
        readonly DeliveryManager _deliveryManager = new DeliveryManager();
        // GET: Factory/Delivery
 
        public ActionResult DeliverableTransferIssueList() 
        {
            IEnumerable<TransferIssue> issueList = _productManager.GetDeliverableTransferIssueList();
            return View(issueList);
        }

        public ActionResult Delivery(int id)
        {
            var deliverable = _productManager.GetDeliverableTransferIssueList().ToList().Find(n => n.TransferIssueId == id);
            ViewBag.Deliverable = deliverable;
            IEnumerable<TransferIssueDetails> issueDetails = _productManager.GetTransferIssueDetailsById(id);
            return View(issueDetails);
        }

        [HttpPost]
        public ActionResult Delivery(int id,FormCollection collection)
        {
            int deliverebyUserId = ((User)Session["user"]).UserId;
            int transferIssueId = id;
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            TransferIssue transferIssue = _productManager.GetDeliverableTransferIssueList().ToList().Find(n => n.TransferIssueId == transferIssueId);
            IEnumerable<TransferIssueDetails> issueDetails = _productManager.GetTransferIssueDetailsById(id);
            Delivery aDelivery = new Delivery();
            aDelivery.TransactionRef = transferIssue.TransferIssueRef;
            aDelivery.DeliveredByUserId = deliverebyUserId;
            aDelivery.Transportation = collection["Transportation"];
            aDelivery.DriverName = collection["DriverName"];
            aDelivery.TransportationCost = Convert.ToDecimal(collection["TransportationCost"]);
            aDelivery.VehicleNo = collection["VehicleNo"];
            aDelivery.DeliveryDate = Convert.ToDateTime(collection["DeliveryDate"]).Date;
            aDelivery.CompanyId = companyId;
            aDelivery.ToBranchId = transferIssue.ToBranchId;
            aDelivery.FromBranchId = transferIssue.FromBranchId;
            string result = _deliveryManager.SaveDeliveryInformation(aDelivery, issueDetails);
            if (result.StartsWith("Sa"))
            {
                return RedirectToAction("DeliverableTransferIssueList");
            }

            ViewBag.Deliverable = transferIssue;
            return View(issueDetails);
        }
    }
}