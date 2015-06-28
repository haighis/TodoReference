using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Akka.Configuration;
using Akka.Actor;
using Akka.Configuration.Hocon;
using Akka.Routing;

namespace WebApplicationSystem1
{
    public class TodoFactory
    {
        public ActorSystem ActorSystem { get; private set; }

        public TodoFactory(ActorSystem system)
        {
            ActorSystem = system;
        }
    }

    public class WebApiApplication : System.Web.HttpApplication
    {
        public static TodoFactory TodoFactory { get; private set; }

        protected void Application_Start()
        {
            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");

            var _clusterConfig = section.AkkaConfig;

            var config =
                   ConfigurationFactory.ParseString("akka.remote.helios.tcp.port=" + 0)
                   //.WithFallback(ConfigurationFactory.ParseString("akka.cluster.roles = [frontend]"))
                       .WithFallback(_clusterConfig);
            
            // Create a client as per documentation 
            var system = ActorSystem.Create("TodoClient", config);

            TodoFactory = new TodoFactory(system);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
