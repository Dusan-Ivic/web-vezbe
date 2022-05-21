using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Zadatak2.Models;

namespace Zadatak2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            List<Product> products = Data.ReadProducts("~/App_Data/products.txt");
            HttpContext.Current.Application["products"] = products;

            List<User> users = Data.ReadUsers("~/App_Data/users.txt");
            HttpContext.Current.Application["users"] = users;
        }
    }
}
