
using Autofac;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using Autofac.Integration.Mvc;
using System.Reflection;
using Business.Interface.Concrete;
using Business.Interface.Abstract;
using Autofac.Integration.WebApi;
using Business;
using Unity;
using Unity.AspNet.Mvc;


namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            var configre = new UnityContainer();
            configre.RegisterType<IUserService, UserService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(configre));
        }
    }
}
