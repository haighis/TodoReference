namespace TodoCommon
{
    public class ActorPaths
    {
        /// <summary>
        /// Todo Actor Path for remote deployment (system1)
        /// </summary>
     
        public const string TodoActorPath = "akka.tcp://system1@localhost:8080/user/todoActor";
     //   public const string DeliveryActorPath = "akka.tcp://system1@localhost:8080/user/todoActor";
    }
}
