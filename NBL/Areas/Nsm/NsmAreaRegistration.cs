using LowercaseRoutesMVC;
using System.Web.Mvc;

namespace NBL.Areas.Nsm
{
    public class NsmAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Nsm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "Nsm_default",
                "Nsm/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}