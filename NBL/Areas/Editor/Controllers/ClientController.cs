using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class ClientController : Controller
    {
        // GET: Editor/Client
        readonly  ClientManager _clientManager=new ClientManager();
        readonly CommonGateway _commonGateway = new CommonGateway();
        readonly DistrictGateway _districtGateway = new DistrictGateway();
        readonly UpazillaGateway _upazillaGateway = new UpazillaGateway();
        readonly PostOfficeGateway _postOfficeGateway = new PostOfficeGateway();
        readonly RegionManager _regionManager=new RegionManager();
        readonly TerritoryManager _territoryManager=new TerritoryManager();
        public ActionResult Details(int id)
        {
            ViewClient client = _clientManager.GetClientDeailsById(id);
            return View(client);

        }

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

        // GET: Sales/Client/AddNewClient
        public ActionResult AddNewClient()
        {
            ViewBag.RegionId= new SelectList(_regionManager.GetAllRegion(), "RegionId", "RegionName");
            ViewBag.ClientTypeId=new SelectList(_commonGateway.GetAllClientType, "ClientTypeId", "ClientTypeName");
            ViewBag.DistrictId = new SelectList(new List<District>(), "DistrictId", "DistrictName");
            ViewBag.UpazillaId = new SelectList(new List<Upazilla>(), "UpazillaId", "UpzillaName");
            ViewBag.PostOfficeId = new SelectList(new List<PostOffice>(), "PostOfficeId", "PostOfficeName");
            ViewBag.TerritoryId = new SelectList(new List<Territory>(), "TerritoryId", "TerritoryName");
            return View();

        }

        // POST: Sales/Client/AddNewClient
        [HttpPost]
        public ActionResult AddNewClient(FormCollection collection, HttpPostedFileBase ClientImage, HttpPostedFileBase clientSignature)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int regionId=Convert.ToInt32(collection["RegionId"]);
            var branch = _regionManager.GetBranchInformationByRegionId(regionId);
            try
            {

                var user = (ViewUser)Session["user"];
                var client = new Client
                {
                    ClientName = collection["ClientName"],
                    CommercialName = collection["CommercialName"],
                    Address = collection["Address"],
                    PostOfficeId = Convert.ToInt32(collection["PostOfficeId"]),
                    ClientTypeId = Convert.ToInt32(collection["ClientTypeId"]),
                    Phone = collection["phone"],
                    AlternatePhone = collection["AlternatePhone"],
                    Gender = collection["Gender"],
                    //Fax = collection["Fax"],
                    //Website = collection["Website"],
                    Email = collection["Email"],
                    CreditLimit = Convert.ToDecimal(collection["CreditLimit"]),
                    UserId = user.UserId,
                    BranchId = branchId,
                    NationalIdNo = collection["NationalIdNo"],
                    TinNo = collection["TinNo"],
                    TerritoryId = Convert.ToInt32(collection["TerritoryId"]),
                    RegionId = regionId,
                    CompanyId = companyId,
                    Branch = branch

                };
                if (ClientImage != null)
                {
                    var ext = Path.GetExtension(ClientImage.FileName);
                    string image = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Client/Photos"), image);
                    // file is uploaded
                    ClientImage.SaveAs(path);
                    client.ClientImage = "Images/Client/Photos/" + image;
                }
                else
                {
                    client.ClientImage = "";
                }
                if (clientSignature != null)
                {
                    string ext = Path.GetExtension(clientSignature.FileName);
                    string sign = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Client/Signatures"), sign);
                    // file is uploaded
                    clientSignature.SaveAs(path);
                    client.ClientSignature = "Images/Client/Signatures/" + sign;
                }
                else
                {
                    client.ClientSignature = "";
                }
                string result = _clientManager.Save(client);

                ViewBag.RegionId = new SelectList(_regionManager.GetAllRegion(), "RegionId", "RegionName");
                ViewBag.ClientTypeId = new SelectList(_commonGateway.GetAllClientType, "ClientTypeId", "ClientTypeName");
                ViewBag.DistrictId = new SelectList(new List<District>(), "DistrictId", "DistrictName");
                ViewBag.UpazillaId = new SelectList(new List<Upazilla>(), "UpazillaId", "UpzillaName");
                ViewBag.PostOfficeId = new SelectList(new List<PostOffice>(), "PostOfficeId", "PostOfficeName");
                ViewBag.TerritoryId = new SelectList(new List<Territory>(), "TerritoryId", "TerritoryName");
                ViewBag.Message = result;
                return View();

            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;
                ViewBag.ClientTypes = _commonGateway.GetAllClientType.ToList();
                ViewBag.Regions = _regionManager.GetAllRegion().ToList();
                return View();
            }
        }

        public ActionResult UploadClientDocument()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadClientDocument(ClientAttachment model,HttpPostedFileBase document) 
        {
             
            if (document != null)
            {
                string fileExtension = Path.GetExtension(document.FileName);
                string doc = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 8) + fileExtension;
                string path = Path.Combine(
                    Server.MapPath("~/ClientDocuments"), doc);
                // file is uploaded
                document.SaveAs(path);
                User anUser = (User)Session["User"];
                model.UploadedByUserId = anUser.UserId;
                model.FilePath = "ClientDocuments/" + doc;
                model.FileExtension = fileExtension;
                bool result = _clientManager.UploadClientDocument(model);
                if (result)
                {
                    ViewBag.Message = "<p style='coler:green'> Uploaded Successfully!</p>";
                }
            }
            else
            {
                ViewBag.Message = "<p style='coler:red'> File upload failed</p>";
            }

            return View();
        }

        public ActionResult ViewClientDocuments()
        {
            IEnumerable<ClientAttachment> attachments = _clientManager.GetClientAttachments();
            return View(attachments);
        }

        
        // GET: Sales/Client/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {

                Client client = _clientManager.GetClientById(id);
                ViewBag.TerritoryId = new SelectList(_territoryManager.GetAllTerritory().ToList().FindAll(n => n.RegionId == client.RegionId), "TerritoryId", "TerritoryName");
                ViewBag.DistrictId = new SelectList(_districtGateway.GetAllDistrictByDivistionId(client.DivisionId),"DistrictId","DistrictName");
                ViewBag.UpazillaId = new SelectList(_upazillaGateway.GetAllUpazillaByDistrictId(client.DistrictId), "UpazillaId", "UpazillaName");
                ViewBag.PostOfficeId = new SelectList(_postOfficeGateway.GetAllPostOfficeByUpazillaId(client.UpazillaId), "PostOfficeId", "PostOfficeName");
                ViewBag.RegionId=new SelectList(_regionManager.GetAllRegion(),"RegionId","RegionName");
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

        // POST: Sales/Client/Edit/5
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
                client.Email = collection["Email"];
                client.CreditLimit = Convert.ToDecimal(collection["CreditLimit"]);
                client.UserId = user.UserId;
                client.NationalIdNo = collection["NationalIdNo"];
                client.TinNo = collection["TinNo"];
                client.TerritoryId = Convert.ToInt32(collection["TerritoryId"]);
                client.RegionId = Convert.ToInt32(collection["RegionId"]);
                Branch aBranch= _regionManager.GetBranchInformationByRegionId(Convert.ToInt32(collection["RegionId"]));
                client.Branch = aBranch;


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
                return RedirectToAction("ViewClient","Home");
            }
            catch(Exception exception)
            {
                Client client = _clientManager.GetClientById(id);
                ViewBag.TerritoryId = new SelectList(_territoryManager.GetAllTerritory().ToList().FindAll(n => n.RegionId == client.RegionId), "TerritoryId", "TerritoryName");
                ViewBag.DistrictId = new SelectList(_districtGateway.GetAllDistrictByDivistionId(client.DivisionId), "DistrictId", "DistrictName");
                ViewBag.UpazillaId = new SelectList(_upazillaGateway.GetAllUpazillaByDistrictId(client.DistrictId), "UpazillaId", "UpazillaName");
                ViewBag.PostOfficeId = new SelectList(_postOfficeGateway.GetAllPostOfficeByUpazillaId(client.UpazillaId), "PostOfficeId", "PostOfficeName");
                ViewBag.RegionId = new SelectList(_regionManager.GetAllRegion(), "RegionId", "RegionName");
                ViewBag.ClientTypeId = new SelectList(_commonGateway.GetAllClientType, "ClientTypeId", "ClientTypeName");
                ViewBag.ErrorMessage = exception.InnerException?.Message;
                return View(client);
              
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