﻿
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles ="User")]
    public class ClientController : Controller
    {
        readonly CommonGateway _commonGateway = new CommonGateway();
        readonly DistrictGateway _districtGateway = new DistrictGateway();
        readonly UpazillaGateway _upazillaGateway = new UpazillaGateway();
        readonly PostOfficeGateway _postOfficeGateway = new PostOfficeGateway();
        readonly ClientManager _clientManager = new ClientManager();
        readonly BranchManager _branchManager = new BranchManager();
        readonly RegionGateway _regionGateway = new RegionGateway();
        readonly TerritoryGateway _territoryGateway = new TerritoryGateway();
        // GET: Sales/Client

        public ActionResult All()
        {

            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                return View(_clientManager.GetClientByBranchId(branchId).ToList().FindAll(n => n.Active == "Y"));
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                throw;
            }

        }

        // GET: Sales/Client/Profile/5

        public PartialViewResult ClientProfile(int id)
        {
            ViewClient client = _clientManager.GetClientDeailsById(id);
           // return View(client);
            return PartialView("_ViewClientProfilePartialPage", client);
        }

        // GET: Sales/Client/AddNewClient
        public ActionResult AddNewClient()
        {
            ViewBag.Regions = _regionGateway.GetAllRegion().ToList();
            ViewBag.ClientTypes = _commonGateway.GetAllClientType.ToList();
            return View();

        }

        // POST: Sales/Client/AddNewClient
        [HttpPost]
        public ActionResult AddNewClient(FormCollection collection, HttpPostedFileBase file, HttpPostedFileBase ClientSignature)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            try
            {

                var user = (User)Session["user"];
                Client client = new Client
                {
                    ClientName = collection["ClientName"],
                    Address = collection["Address"],
                    PostOfficeId = Convert.ToInt32(collection["PostOfficeId"]),
                    ClientTypeId = Convert.ToInt32(collection["ClientTypeId"]),
                    Phone = collection["phone"],
                    AlternatePhone = collection["AlternatePhone"],
                    Gender = collection["Gender"],
                    Fax = collection["Fax"],
                    Email = collection["Email"],
                    Website = collection["Website"],
                    UserId = user.UserId,
                    BranchId = branchId,
                    NationalIdNo = collection["NationalIdNo"],
                    TinNo = collection["TinNo"],
                    TerritoryId = Convert.ToInt32(collection["TerritoryId"]),
                    RegionId = Convert.ToInt32(collection["RegionId"]),
                    CompanyId = companyId

                };
                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    string image = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Client/Photos"), image);
                    // file is uploaded
                    file.SaveAs(path);
                    client.ClientImage = "Images/Client/Photos/" + image;
                }
                else
                {
                    client.ClientImage = "";
                }
                if (ClientSignature != null)
                {
                    string ext = Path.GetExtension(ClientSignature.FileName);
                    string sign = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Client/Signatures"), sign);
                    // file is uploaded
                    ClientSignature.SaveAs(path);
                    client.ClientSignature = "Images/Client/Signatures/" + sign;
                }
                else
                {
                    client.ClientSignature = "";
                }
                string result = _clientManager.Save(client);
                ViewBag.ClientTypes = _commonGateway.GetAllClientType.ToList();
                ViewBag.Regions = _regionGateway.GetAllRegion().ToList();
                ViewBag.Message = result;
                return View();

            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;
                ViewBag.ClientTypes = _commonGateway.GetAllClientType.ToList();
                ViewBag.Regions = _regionGateway.GetAllRegion().ToList();
                return View();
            }
        }

        // GET: Sales/Client/Edit/5
        public ActionResult Edit(int id)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var branch = _branchManager.GetBranchById(branchId);
            try
            {

                Client client = _clientManager.GetClientById(id);
                ViewBag.Territories = _territoryGateway.GetAllTerritory().ToList().FindAll(n => n.RegionId == client.RegionId).ToList();
                ViewBag.Districts = _districtGateway.GetAllDistrictByDivistionId(client.DivisionId);
                ViewBag.Upazillas = _upazillaGateway.GetAllUpazillaByDistrictId(client.DistrictId);
                ViewBag.PostOffices = _postOfficeGateway.GetAllPostOfficeByUpazillaId(client.UpazillaId);
                ViewBag.Regions = _regionGateway.GetAllRegion().ToList();
                ViewBag.ClientTypes = _commonGateway.GetAllClientType.ToList();
                return View(client);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    ViewBag.Msg = e.InnerException.Message;
                return View();
            }

        }

        // POST: Sales/Client/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase file, HttpPostedFileBase ClientSignature)
        {
            try
            {
                var user = (User)Session["user"];
                Client client = _clientManager.GetClientById(id);
                client.ClientName = collection["ClientName"];
                client.Address = collection["Address"];
                client.PostOfficeId = Convert.ToInt32(collection["PostOfficeId"]);
                client.ClientTypeId = Convert.ToInt32(collection["ClientTypeId"]);
                client.Phone = collection["phone"];
                client.AlternatePhone = collection["AlternatePhone"];
                client.Gender = collection["Gender"];
                client.Fax = collection["Fax"];
                client.Email = collection["Email"];
                client.Website = collection["Website"];
                client.UserId = user.UserId;
                client.NationalIdNo = collection["NationalIdNo"];
                client.TinNo = collection["TinNo"];
                client.TerritoryId = Convert.ToInt32(collection["TerritoryId"]);
                client.RegionId = Convert.ToInt32(collection["RegionId"]);

                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    string image = "cp" + Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Client/Photos"), image);
                    // file is uploaded
                    file.SaveAs(path);
                    client.ClientImage = "Images/Client/Photos/" + image;
                }
                if (ClientSignature != null)
                {
                    string ext = Path.GetExtension(ClientSignature.FileName);
                    string sign = "cs" + Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Client/Signatures"), sign);
                    // file is uploaded
                    ClientSignature.SaveAs(path);
                    client.ClientSignature = "Images/Client/Signatures/" + sign;
                }
                string result = _clientManager.Update(id, client);
                return RedirectToAction("All");
            }
            catch
            {
                return View();
            }
        }

        public JsonResult ClientEmailExists(string email)
        {

            Client client = _clientManager.GetClientByEmailAddress(email);
            if (client.Email != null)
            {
                client.EmailInUse = true;
            }
            else
            {
                client.EmailInUse = false;
                client.Email = email;
            }
            return Json(client);
        }
       
    }
}