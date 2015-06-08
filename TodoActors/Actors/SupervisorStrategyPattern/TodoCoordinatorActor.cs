using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using TodoActors.Actors.SupervisorStrategyPattern;
using TodoDataModel;

namespace TodoActors.Actors
{
    public class TodoCoordinatorActor : ReceiveActor
    {
        /// <summary>
        /// Restart any children who throw an <see cref="UnknownTodoException"/> message.
        /// </summary>
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(1000,10,Decider.From(Directive.Restart,
                new KeyValuePair<Type, Directive>(typeof(UnknownTodoException), Directive.Restart)));
        }

        public TodoCoordinatorActor()
        {
            Receive<Message>(msg =>
            {
                var todoChildActor = LookupOrCreateTodoChildActor("todo");
                todoChildActor.Forward(msg);
            });
        }

        private IActorRef LookupOrCreateTodoChildActor(string name)
        {
            return LookupOrCreateTodoChildActor(Context, name);
        }

        private IActorRef LookupOrCreateTodoChildActor(IActorContext context, string name)
        {
            var child = context.Child(name);
            if (child.Equals(ActorRefs.Nobody)) //child doesn't exist
            {
                return Context.ActorOf(Props.Create(() => new TodoChildActor()),
                    name);
            }
            return child;
        }
    }
}
