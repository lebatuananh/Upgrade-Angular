using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace DVG.CK.OMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(IRouteBuilder routes)
        {

            routes.MapRoute(
                name: "Login",
                template: "dang-nhap",
                defaults: new { controller = "Account", action = "Login" }
                );

            routes.MapRoute(
                name: "ChangePassword",
                template: "doi-mat-khau",
                defaults: new { controller = "Account", action = "Manager" }
            );

            routes.MapRoute(
                name: "Logout",
                template: "dang-xuat",
                defaults: new { controller = "Account", action = "Logout" }
                );

            routes.MapRoute(
                name: "AccessDenied",
                template: "access-denied",
                defaults: new { controller = "Account", action = "PermissionDenied" }
                );

            routes.MapRoute(
                name: "Default",
                template: "{controller=Home}/{action=Index}/{id?}"
            );
        }
    }
}