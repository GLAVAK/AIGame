using GameServer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace GameServer
{
    public static class WebApiConfig
    {
        public static string UrlPrefix { get { return "api"; } }
        public static string UrlPrefixRelative { get { return "~/api"; } }

        public static void Register(RouteCollection routes)
        {
            var route = routes.MapHttpRoute(
                name: "ApiActionRoute",
                routeTemplate: WebApiConfig.UrlPrefix + "/{controller}/{action}",
                defaults: new { action = "Index" }
            );

            route.RouteHandler = new MyHttpControllerRouteHandler();
        }
    }

    //Following code is to enable Session support in API Controllers
    public class MyHttpControllerHandler
        : HttpControllerHandler, IRequiresSessionState
    {
        public MyHttpControllerHandler(RouteData routeData)
            : base(routeData) { }
    }

    public class MyHttpControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new MyHttpControllerHandler(requestContext.RouteData);
        }
    }
}
