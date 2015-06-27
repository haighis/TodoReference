using System;
using Akka.Actor;
using TodoActors.Actors;
using TodoCommon;
using TodoDataModel;

namespace TodoActorService
{
    //TODO Does this need to be refactored to use IActorRef's instead of actor selection???
    public class TodosActorService
    {
        private readonly ActorSystem _actorSystem;

        public TodosActorService(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }

        public void SendTodo(string taskName)
        {
            var todoCoordinator = _actorSystem.ActorSelection(ActorPaths.CoordinatorPath); 
            todoCoordinator.Tell(new Message(taskName));
        }
    }
}
