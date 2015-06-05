using Akka.Actor;
using TodoCommon;

namespace TodoActorService
{
    //TODO this needs to be refactored to use IActorRef's instead of actor selection
    public class TodosActorService
    {
        private readonly ActorSystem _actorSystem;

        public TodosActorService(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }

        public void SendTodo(string taskName)
        {
            var test = _actorSystem.ActorSelection(ActorPaths.TodoActorPath);

            test.Tell("print");
            // restart and recovery
            test.Tell("boom");
            test.Tell("print");
            test.Tell("b");
            test.Tell("print");
            test.Tell("c");
            test.Tell("print");
        }
    }
}
