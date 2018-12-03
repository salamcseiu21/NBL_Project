using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;

namespace NBL.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "Super")]
    public class ApproveController : Controller
    {
        readonly CommonGateway _commonGateway = new CommonGateway();
        readonly DistrictGateway _districtGateway = new DistrictGateway();
        readonly UpazillaGateway _upazillaGateway = new UpazillaGateway();
        readonly PostOfficeGateway _postOfficeGateway = new PostOfficeGateway();
        readonly ClientManager _clientManager = new ClientManager();
        readonly RegionGateway _regionGateway = new RegionGateway();
        readonly TerritoryGateway _territoryGateway = new TerritoryGateway();
        // GET: SuperAdmin/Approve
        public ActionResult ApproveClient() 
        {
            List<Client> clients = _clientManager.GetPendingClients();
            return View(clients);
        }
        [HttpPost]
        public ActionResult ApproveClient(FormCollection collection)
        {

            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {

                var anUser = (ViewUser)Session["user"];
                int clientId = Convert.ToInt32(collection["ClientId"]);
                var aClient = _clientManager.GetClientById(clientId);
                bool result = _clientManager.ApproveClient(aClient,anUser);
                aModel.Message = result ? "<p class='text-green'> Client Approved Successfully!</p>" : "<p class='text-danger'> Failed to  Approve Client ! </p>";
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                aModel.Message = " <p style='color:red'>" + message + "</p>";

            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {

                Client client = _clientManager.GetClientById(id);
                ViewBag.TerritoryId = new SelectList(_territoryGateway.GetAllTerritory().ToList().FindAll(n => n.RegionId == client.RegionId), "TerritoryId", "TerritoryName");
                ViewBag.DistrictId = new SelectList(_districtGateway.GetAllDistrictByDivistionId(client.DivisionId), "DistrictId", "DistrictName");
                ViewBag.UpazillaId = new SelectList(_upazillaGateway.GetAllUpazillaByDistrictId(client.DistrictId), "UpazillaId", "UpazillaName");
                ViewBag.PostOfficeId = new SelectList(_postOfficeGateway.GetAllPostOfficeByUpazillaId(client.UpazillaId), "PostOfficeId", "PostOfficeName");
                ViewBag.RegionId = new SelectList(_regionGateway.GetAllRegion(), "RegionId", "RegionName");
                ViewBag.ClientTypeId = new SelectList(_commonGateway.GetAllClientType, "ClientTypeId", "ClientTypeName");
                return View(client);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    ViewBag.Msg = e.InnerException.Message;
                return View();
            }

        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase file, HttpPostedFileBase ClientSignature)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                Client client = _clientManager.GetClientById(id);
                client.ClientName = collection["ClientName"];
                client.Address = collection["Address"];
                client.PostOfficeId = Convert.ToInt32(collection["PostOfficeId"]);
                client.ClientTypeId = Convert.ToInt32(collection["ClientTypeId"]);
                client.Phone = collection["phone"];
                client.AlternatePhone = collection["AlternatePhone"];
                client.Gender = collection["Gender"];
                //client.Fax = collection["Fax"];
                //client.Website = collection["Website"];
                client.Email = collection["Email"];
                client.CreditLimit = Convert.ToDecimal(collection["CreditLimit"]);
                client.UserId = user.UserId;
                client.NationalIdNo = collection["NationalIdNo"];
                client.TinNo = collection["TinNo"];
                client.TerritoryId = Convert.ToInt32(collection["TerritoryId"]);
                client.RegionId = Convert.ToInt32(collection["RegionId"]);
                client.Active = "Y";

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
                return RedirectToAction("ApproveClient");
            }
            catch
            {
                return View();
            }
        }
    }
}