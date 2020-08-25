using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace DVG.CK.OMSApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "AhaMoveCallback",
                template: "ahamove-callback",
                defaults: new { controller = "Orders", action = "AhaMoveCallback" }
                );
        }
    }
}