using DotNetNuke.Web.Api;


namespace JEOLUSA.UXManager.Extensions.Menu.RoleAssignment.Controllers
{
    public class ServiceRouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute("RoleAssignment", "default", "{controller}/{action}", new[] { "JEOLUSA.UXManager.Extensions.Menu.RoleAssignment.Controllers" });
        }
    }
}

