using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class ProductController : Controller
    {

        readonly  CommonGateway _commonGateway=new CommonGateway();
        readonly CompanyManager _companyManager=new CompanyManager();
        readonly ProductManager _productManager=new ProductManager();
        // GET: Editor/Product
        public ActionResult AddProduct() 
        {
            var categories = _commonGateway.GetAllProductCategory.ToList();
            var types = _commonGateway.GetAllProductType.ToList();
            var companies = _companyManager.GetAll.ToList();
            ViewBag.Types = types;
            ViewBag.Companies = companies;
            return View(categories);
        }
        [HttpPost]
        public ActionResult AddProduct(FormCollection collection, HttpPostedFileBase ProductImage)
        {
            try
            {

                Product aProduct = new Product
                {
                    CategoryId = Convert.ToInt32(collection["ProductCategoryId"]),
                    CompanyId = Convert.ToInt32(collection["CompanyId"]),
                    ProductTypeId = Convert.ToInt32(collection["ProductTypeId"]),
                    ProductName = collection["ProductName"],
                    Unit = collection["Unit"],
                    ProductAddedDate = Convert.ToDateTime(collection["ProductAddedDate"])
                };
                if (ProductImage != null)
                {
                    string ext = Path.GetExtension(ProductImage.FileName);
                    string image = Guid.NewGuid().ToString().Replace("-", "").ToLower() + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Areas/Editor/Images/Products"), image);
                    // file is uploaded
                    ProductImage.SaveAs(path);
                    aProduct.ProductImage = image;
                }

                string result = _productManager.Save(aProduct);
                TempData["Message"] = result;
                var categories = _commonGateway.GetAllProductCategory.ToList();
                var types = _commonGateway.GetAllProductType.ToList();
                var companies = _companyManager.GetAll.ToList();
                ViewBag.Types = types;
                ViewBag.Companies = companies;
                return View(categories);
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                var categories = _commonGateway.GetAllProductCategory.ToList();
                var types = _commonGateway.GetAllProductType.ToList();
                var companies = _companyManager.GetAll.ToList();
                ViewBag.Types = types;
                ViewBag.Companies = companies;
                return View(categories);
            }
        }
    }
}