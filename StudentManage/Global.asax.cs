using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StudentManage.App_Start;
using System.Data.Entity;
using StudentManage.Models;
using System.Web.Http;
using StudentManage.Manage;

namespace StudentManage
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        //{
        //    //filters.Add(new HandleErrorAttribute());            
        //}

        protected void Application_Start()
        {
            //string licenseName = "79;100-ZZZ";//... PRO license name
            //string licenseKey = "270D6859908C96DBC9758C2E7512856E";//... PRO license key
            //Z.EntityFramework.Extensions.LicenseManager.AddLicense(licenseName, licenseKey);

            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BaseContext>());
            Database.SetInitializer<BaseContext>(null);
            AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            //HandlerConfig.RegisterHandlers(GlobalConfiguration.Configuration.MessageHandlers);
            ScanFoundDelTask t = new ScanFoundDelTask();

        }

        
        //protected void Application_Error(object sender, EventArgs e)
        //{
           // Exception exception = Server.GetLastError();
           // Response.Clear();
           // HttpException httpException = exception as HttpException;
           // RouteData routeData = new RouteData();
           // routeData.Values.Add("controller", "Error");
           // if (httpException == null)
           // {
           //     routeData.Values.Add("action", "Index");
           // }
           // else //It's an Http Exception, Let's handle it.  
           // {
           //     switch (httpException.GetHttpCode())
           //     {
           //         case 404:
           //             // Page not found.  
           //             routeData.Values.Add("action", "HttpError404");
           //             break;
           //         case 500:
           //             // Server error.  
           //             routeData.Values.Add("action", "HttpError500");
           //             break;
           //         // Here you can handle Views to other error codes.  
           //         // I choose a General error template    
           //         default:
           //             routeData.Values.Add("action", "General");
           //             break;
           //     }
           // }
           // // Pass exception details to the target error View.  
           // routeData.Values.Add("error", exception.Message);
           // // Clear the error on server.  
           // Server.ClearError();
           // // Call target Controller and pass the routeData.  
           // IController errorController = new StudentManage.Controllers.ErrorController();
           // errorController.Execute(new RequestContext(
           //new HttpContextWrapper(Context), routeData));
        //}


    }
}
