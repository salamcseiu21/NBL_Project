﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Factory.BLL;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Factory.Controllers
{
    [Authorize(Roles ="Factory")]
    public class DeliveryController : Controller
    {

        private readonly IProductManager _iProductManager;
        private readonly DeliveryManager _deliveryManager = new DeliveryManager();
        // GET: Factory/Delivery
        public DeliveryController(IProductManager iProductManager)
        {
            _iProductManager = iProductManager;
        }
        public ActionResult DeliverableTransferIssueList() 
        {
            IEnumerable<TransferIssue> issueList = _iProductManager.GetDeliverableTransferIssueList();
            return View(issueList);
        }

        public ActionResult Delivery(int id)
        {
            var deliverable = _iProductManager.GetDeliverableTransferIssueList().ToList().Find(n => n.TransferIssueId == id);
            ViewBag.Deliverable = deliverable;
            IEnumerable<TransferIssueDetails> issueDetails = _iProductManager.GetTransferIssueDetailsById(id);
            return View(issueDetails);
        }

        [HttpPost]
        public ActionResult Delivery(int id,FormCollection collection)
        {
            int deliverebyUserId = ((ViewUser)Session["user"]).UserId;
            int transferIssueId = id;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            TransferIssue transferIssue = _iProductManager.GetDeliverableTransferIssueList().ToList().Find(n => n.TransferIssueId == transferIssueId);
            IEnumerable<TransferIssueDetails> issueDetails = _iProductManager.GetTransferIssueDetailsById(id);
            Delivery aDelivery = new Delivery
            {
                TransactionRef = transferIssue.TransferIssueRef,
                DeliveredByUserId = deliverebyUserId,
                Transportation = collection["Transportation"],
                DriverName = collection["DriverName"],
                TransportationCost = Convert.ToDecimal(collection["TransportationCost"]),
                VehicleNo = collection["VehicleNo"],
                DeliveryDate = Convert.ToDateTime(collection["DeliveryDate"]).Date,
                CompanyId = companyId,
                ToBranchId = transferIssue.ToBranchId,
                FromBranchId = transferIssue.FromBranchId
            };

            string result = _deliveryManager.SaveDeliveryInformation(aDelivery, issueDetails);
            if (result.StartsWith("Sa"))
            {
                //---------------Send mail to branch before redirect--------------
                return RedirectToAction("DeliverableTransferIssueList");
            }

            ViewBag.Deliverable = transferIssue;
            return View(issueDetails);
        }
    }
}