using DVG.WIS.WebApp.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace DVG.WIS.WebApp.Configs
{
    public class RouteConfig
    {
        public static void RegisterRoutes(IRouteBuilder routes)
        {
            //var allConfig = RouteHelper.Instance.GetAll();
            //var dicDefault = new RouteValueDictionary();
            //foreach (var route in allConfig.Route)
            //{
            //    if(route.Defaults != null)
            //    {
            //        foreach (var item in route.Defaults)
            //        {
            //            if (item.HasValues)
            //            {
            //                dicDefault.Add(item.Name + "", item.Value + "");
            //            }
            //        }
            //    }

            //    routes.MapRoute(
            //      name: route.Name,
            //      template: route.Url,
            //      defaults: dicDefault
            //      //constraints: dicConstraints
            //    );
            //}

            routes.MapRoute(
              name: "Contact",
              template: "lien-he",
              defaults: new { controller = "Home", action = "Contact" }
            );

            routes.MapRoute(
              name: "About",
              template: "ve-chung-toi",
              defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
              name: "Product Detail",
              template: "{url}-p{id}",
              defaults: new { controller = "Product", action = "Detail" },
              constraints: new { id=@"[0-9]+" }
            );

            routes.MapRoute(
              name: "Article Detail",
              template: "{url}-n{id}",
              defaults: new { controller = "Article", action = "Detail" },
              constraints: new { id = @"[0-9]+" }
            );

            routes.MapRoute(
              name: "Product List",
              template: "san-pham/{curl}",
              defaults: new { controller = "Product", action = "Index" },
              constraints: new { curl = @"[a-z0-9\-]+" }
            );

            routes.MapRoute(
              name: "Article List - Page",
              template: "{curl}/p{page}",
              defaults: new { controller = "Article", action = "Index" },
              constraints: new { curl = @"[a-z0-9\-]+", page = @"[0-9]+" }
            );
            routes.MapRoute(
              name: "Article List",
              template: "{curl}",
              defaults: new { controller = "Article", action = "Index" },
              constraints: new { curl = @"[a-z0-9\-]+" }
            );

            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}"
            );
        }
    }
}