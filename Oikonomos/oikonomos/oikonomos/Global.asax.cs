using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using oikonomos.repositories;


namespace oikonomos.web
{
    public class MvcApplication : HttpApplication
    {
        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Login", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.Add(new Route("{controller}.mvc.aspx/{action}", new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(new { controller = "YourController" })
            });
        }

        private static void RegisterWebApi(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            ModelMetadataProviders.Current = new DataAnnotationsModelMetadataProvider();
            
            AreaRegistration.RegisterAllAreas();

            RegisterWebApi(GlobalConfiguration.Configuration);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.IsSecureConnection.Equals(false) && HttpContext.Current.Request.Url.Host!="localhost")
            {
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl);
            }
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            CurrentContext.EndSession(Session.SessionID);
        }

    }
}