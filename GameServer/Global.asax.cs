using DataBase;
using GameServer.Concrete;
using GameServer.GameLogic;
using GameServer.Infrastructure;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace GameServer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Set up Unity DI:
            var container = new UnityContainer();
            ContainerBootstrapper.RegisterTypes(container);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityResolver(container);

            //Start thread that executes user code every second
            WorldUpdateWrapper.StartWorldUpdate(container);

            //Initialize database with test data
            Database.SetInitializer<GameServerContext>(new DBInitializer());
        }

    }
}