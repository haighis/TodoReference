namespace TodoCommon
{
    /// <summary>
    /// Todo Actor Path for remote deployment (system1)
    /// </summary>
    public class ActorPaths
    {   
        public const string TodoActorPath = "akka.tcp://system1@localhost:8080/user/todoActor";
        //public const string CoordinatorPath = "akka://system1/user/todocoordinator";

        public const string CoordinatorPath = "akka.tcp://system1@127.0.0.1:2551/user/todocoordinator";
    }
}
