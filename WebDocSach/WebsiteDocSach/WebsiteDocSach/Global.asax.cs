using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebsiteDocSach
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["ActiveUsers"] = 0;
        }

        protected void Application_End()
        {
            Application["ActiveUsers"] = 0;
            Session["count"] = 0;
        }


        void Session_Start()
        {
            if (Session["online"] == null)
            {
                Session["online"] = 1;
            }
            else
            {
                Session["online"] = int.Parse(Session["online"].ToString()) + 1;
            }

            Session["Start"] = DateTime.Now;


            Application.Lock();

            Application["ActiveUsers"] = (int)Application["ActiveUsers"] + 1;
            Session["count"] = Application["ActiveUsers"];
            Application.UnLock();
        }

        void Session_End()
        {
            Session["online"] = int.Parse(Session["online"].ToString()) - 1;
            Application.Lock();

            Application["ActiveUsers"] = 0;
            Session["count"] = Application["ActiveUsers"];

            Application.UnLock();

            Session.Clear();

            Session.Remove("Start");
        }
    }
}
