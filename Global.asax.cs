using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServerContainer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ServerContainer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //  Database.SetInitializer<ApplicationDbContext>(new AppDbInitializer());
            //var r
                      //    = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>()).Create(new IdentityRole { Name = "admin" });
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
