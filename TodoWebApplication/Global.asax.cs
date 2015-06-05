using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Akka.Configuration;
using Akka.Actor;
using Akka.Routing;

namespace WebApplicationSystem1
{
    public class MasterCatalogFactory
    {
        public ActorSystem ActorSystem { get; private set; }

        public MasterCatalogFactory(ActorSystem system)
        {
            ActorSystem = system;
        }
    }

    public class WebApiApplication : System.Web.HttpApplication
    {
        public static MasterCatalogFactory MasterCatalogFactory { get; private set; }

        protected void Application_Start()
        {
            var config = ConfigurationFactory.ParseString(@"
akka {  
    log-config-on-start = on
    stdout-loglevel = DEBUG
    loglevel = DEBUG
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
        
        debug {  
          receive = on 
          autoreceive = on
          lifecycle = on
          event-stream = on
          unhandled = on
        }
    }
    remote {
        helios.tcp {
		    port = 8090
		    hostname = localhost
        }
    }
}
");
            // Create a client as per documentation - http://getakka.net/docs/Remoting
            var system = ActorSystem.Create("TodoClient", config);

            MasterCatalogFactory = new MasterCatalogFactory(system);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
