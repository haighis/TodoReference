using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Routing;
using TodoDataModel;

namespace TodoActors.Actors
{
    public interface IMessageCommand : IConsistentHashable
    {
        Message Message { get; }

        IActorRef Requestor { get; }
    }
}
