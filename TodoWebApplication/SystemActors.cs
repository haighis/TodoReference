using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Akka.Actor;

namespace WebApplicationSystem1
{
    public static class SystemActors
    {
        public static IActorRef CommandProcessor = ActorRefs.Nobody;
        public static IActorRef TodoCoordinator = ActorRefs.Nobody;
    }
}