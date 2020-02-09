using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TierraCreative
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "LoginDefault",
              url: "",
              defaults: new { controller = "users", action = "login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
             name: "Logout",
             url: "logout",
             defaults: new { controller = "users", action = "logout", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Login",
              url: "login",
              defaults: new { controller = "users", action = "login", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
             name: "Forms",
             url: "forms",
             defaults: new { controller = "users", action = "forms", id = UrlParameter.Optional }
           );


            routes.MapRoute(
             name: "ChangePassword",
             url: "changepassword",
             defaults: new { controller = "users", action = "changepassword", id = UrlParameter.Optional }
           );

            routes.MapRoute(
             name: "ForgotPassword",
             url: "forgotpassword",
             defaults: new { controller = "users", action = "forgotpassword", id = UrlParameter.Optional }
           );

            routes.MapRoute(
           name: "ForgotPasswordChange",
           url: "ForgotPasswordChange",
           defaults: new { controller = "users", action = "forgotpasswordchange", id = UrlParameter.Optional }
         );

            routes.MapRoute(
             name: "ReviewApproves",
             url: "ra",
             defaults: new { controller = "reviewapproves", action = "review", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "ReviewApprovesWAction",
              url: "ra/{action}/{id}",
              defaults: new { controller = "reviewapproves", action = "review", id = UrlParameter.Optional }
           );

            #region -- Administration --

            routes.MapRoute(
               name: "AdminLogin",
               url: "admin",
               defaults: new { controller = "users", action = "admin" }
            );

            routes.MapRoute(
               name: "AdminMain",
               url: "admin/{action}/{id}",
               defaults: new { controller = "users", action = "main", id = UrlParameter.Optional }
            );

            // routes.MapRoute(
            //   name: "AdminReport",
            //   url: "admin/{action}/{code}",
            //   defaults: new { controller = "administrations", action = "CustomerSalesReportGraph", code = "1111" }
            //);

            #endregion           


            routes.MapRoute(
              name: "PageError",
              url: "PageError/{action}/{id}",
              defaults: new { controller = "users", action = "PageError", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
