using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class ClientController : Controller
    {
        // GET: Editor/Client
        private readonly  IClientManager _iClientManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IDistrictManager _iDistrictManager;
        readonly UpazillaGateway _upazillaGateway = new UpazillaGateway();
        readonly PostOfficeGateway _postOfficeGateway = new PostOfficeGateway();
        readonly IRegionManager _iRegionManager;
        readonly ITerritoryManager _iTerritoryManager;

        public ClientController(IClientManager iClientManager,ICommonManager iCommonManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager,IDistrictManager iDistrictManager)
        {
            _iClientManager = iClientManager;
            _iCommonManager = iCommonManager;
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
            _iDistrictManager = iDistrictManager;
        }
        public ActionResult Details(int id)
        {
            ViewClient client = _iClientManager.GetClientDeailsById(id);
            return View(client);

        }

        public ActionResult All()
        {

            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                return View(_iClientManager.GetClientByBranchId(branchId).ToList().FindAll(n => n.Active == "Y"));
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
            ViewBag.RegionId= new SelectList(_iRegionManager.GetAll(), "RegionId", "RegionName");
            ViewBag.ClientTypeId=new SelectList(_iCommonManager.GetAllClientType(), "ClientTypeId", "ClientTypeName");
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
            var branch = _iRegionManager.GetBranchInformationByRegionId(regionId);
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
                    //Branch = branch

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
                string result = _iClientManager.Save(client);
                ViewBag.RegionId = new SelectList(_iRegionManager.GetAll(), "RegionId", "RegionName");
                ViewBag.ClientTypeId = new SelectList(_iCommonManager.GetAllClientType(), "ClientTypeId", "ClientTypeName");
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
                ViewBag.ClientTypes = _iCommonManager.GetAllClientType().ToList();
                ViewBag.Regions = _iRegionManager.GetAll().ToList();
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
                var anUser = (ViewUser)Session["User"];
                model.UploadedByUserId = anUser.UserId;
                model.FilePath = "ClientDocuments/" + doc;
                model.FileExtension = fileExtension;
                bool result = _iClientManager.UploadClientDocument(model);
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
            IEnumerable<ClientAttachment> attachments = _iClientManager.GetClientAttachments();
            return View(attachments);
        }

        
        // GET: Sales/Client/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {

                Client client = _iClientManager.GetClientById(id);
                ViewBag.TerritoryId = new SelectList(_iTerritoryManager.GetAll().ToList().FindAll(n => n.RegionId == client.RegionId), "TerritoryId", "TerritoryName");
                ViewBag.DistrictId = new SelectList(_iDistrictManager.GetAllDistrictByDivistionId(client.DivisionId),"DistrictId","DistrictName");
                ViewBag.UpazillaId = new SelectList(_upazillaGateway.GetAllUpazillaByDistrictId(client.DistrictId), "UpazillaId", "UpazillaName");
                ViewBag.PostOfficeId = new SelectList(_postOfficeGateway.GetAllPostOfficeByUpazillaId(client.UpazillaId), "PostOfficeId", "PostOfficeName");
                ViewBag.RegionId=new SelectList(_iRegionManager.GetAll(),"RegionId","RegionName");
                ViewBag.ClientTypeId = new SelectList(_iCommonManager.GetAllClientType(), "ClientTypeId", "ClientTypeName");
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
                Client client = _iClientManager.GetClientById(id);
                client.ClientName = collection["ClientName"];
                client.CommercialName= collection["CommercialName"];
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
                Branch aBranch= _iRegionManager.GetBranchInformationByRegionId(Convert.ToInt32(collection["RegionId"]));
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
               
                string result = _iClientManager.Update(id, client);
                return RedirectToAction("ViewClient","Home");
            }
            catch(Exception exception)
            {
                Client client = _iClientManager.GetClientById(id);
                ViewBag.TerritoryId = new SelectList(_iTerritoryManager.GetAll().ToList().FindAll(n => n.RegionId == client.RegionId), "TerritoryId", "TerritoryName");
                ViewBag.DistrictId = new SelectList(_iDistrictManager.GetAllDistrictByDivistionId(client.DivisionId), "DistrictId", "DistrictName");
                ViewBag.UpazillaId = new SelectList(_upazillaGateway.GetAllUpazillaByDistrictId(client.DistrictId), "UpazillaId", "UpazillaName");
                ViewBag.PostOfficeId = new SelectList(_postOfficeGateway.GetAllPostOfficeByUpazillaId(client.UpazillaId), "PostOfficeId", "PostOfficeName");
                ViewBag.RegionId = new SelectList(_iRegionManager.GetAll(), "RegionId", "RegionName");
                ViewBag.ClientTypeId = new SelectList(_iCommonManager.GetAllClientType(), "ClientTypeId", "ClientTypeName");
                ViewBag.ErrorMessage = exception.InnerException?.Message;
                return View(client);
              
            }
        }

        public JsonResult ClientEmailExists(string email)
        {

            Client client = _iClientManager.GetClientByEmailAddress(email);
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