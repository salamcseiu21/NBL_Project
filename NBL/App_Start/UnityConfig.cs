using System.Web.Mvc;
using NBL.Areas.Manager.BLL;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using Unity;
using Unity.Mvc5;

namespace NBL
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            container.RegisterType<IBranchGateway, BranchGateway>();
            container.RegisterType<IBranchManager, BranchManager>();
            container.RegisterType<IClientGateway, ClientGateway>();
            container.RegisterType<IClientManager, ClientManager>();
            container.RegisterType<ICommonGateway, CommonGateway>();
            container.RegisterType<ICompanyGateway, CompanyGateway>();
            container.RegisterType<ICompanyManager, CompanyManager>();
            container.RegisterType<IDepartmentManager, DepartmentManager>();
            container.RegisterType<IDepartmentGateway, DepartmentGateway>();
            container.RegisterType<IDesignationGateway, DesignationGateway>();
            container.RegisterType<IDesignationManager, DesignationManager>();
            container.RegisterType<IDiscountGateway, DiscountGateway>();
            container.RegisterType<IDiscountManager, DiscountManager>();

            container.RegisterType<IDistrictGateway, DistrictGateway>();
            container.RegisterType<IDivisionGateway, DivisionGateway>();

            container.RegisterType<IEmployeeGateway, EmployeeGateway>();
            container.RegisterType<IEmployeeManager, EmployeeManager>();
            container.RegisterType<IEmployeeTypeGateway, EmployeeTypeGateway>();
            container.RegisterType<IEmployeeTypeManager, EmployeeTypeManager>();

            container.RegisterType<IInventoryGateway, InventoryGateway>();
            container.RegisterType<IInventoryManager, InventoryManager>();

            container.RegisterType<IOrderGateway, OrderGateway>();
            container.RegisterType<IOrderManager, OrderManager>();

            container.RegisterType<IPostOfficeGateway, PostOfficeGateway>();
            container.RegisterType<IProductManager, ProductManager>();

            container.RegisterType<IProductGateway, ProductGateway>();
            container.RegisterType<IRegionGateway, RegionGateway>();
            container.RegisterType<IRegionManager, RegionManager>();

            container.RegisterType<IReportGateway, ReportGateway>();
            container.RegisterType<IReportManager, ReportManager>();
            container.RegisterType<ITerritoryGateway, TerritoryGateway>();
            container.RegisterType<ITerritoryManager, TerritoryManager>();
            container.RegisterType<IUpazillaGateway, UpazillaGateway>();
            container.RegisterType<IUserGateway, UserGateway>();
            container.RegisterType<IUserManager, UserManager>();

            container.RegisterType<IVatGateway, VatGateway>();
            container.RegisterType<IVatManager, VatManager>();
            container.RegisterType<IDeliveryManager, DeliveryManager>();
            
        }
    }
}