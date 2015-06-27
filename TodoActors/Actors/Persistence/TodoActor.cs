using System;
using System.Linq;
using Akka.Actor;

using Akka.Persistence;
using TodoCommon;
using TodoDataModel;
using TodoService;

namespace TodoActors.Actors
{
    /// <summary>
    /// Todo Actor is an at least once delivery actor.
    /// 
    /// TODO add business logic for validation of todo business rules. 
    /// </summary>
    public class TodoActor : AtLeastOnceDeliveryActor
    {
        //TODO move this to be injected to the constructor, dependency injection etc. 
        private ITodoServiceBusinessLogic _todoService;

        public ActorPath DeliveryPath { get; private set; }

        public TodoActor(ITodoServiceBusinessLogic todoServiceBusinessLogic, ActorPath deliveryPath)
            : this(deliveryPath)
        {
            _todoService = todoServiceBusinessLogic;
        }

        public TodoActor(ActorPath deliveryPath)
        {
            this.DeliveryPath = deliveryPath;
            _todoService = new TodoServiceBusinessLogic();
        }

        /// <summary>
        /// Define Persistence Id
        /// </summary>
        public override string PersistenceId
        {
            get { return "atleastonce-1"; }
        }

        /// <summary>
        /// Receive Recovers will process messages sent when the Actor restarts. 
        /// With a durable store such as MS SQL, Cassandra or Postgres
        /// upon actor restart messages will be processed here.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override bool ReceiveRecover(object message)
        {
            if (message is Message)
            {
                var messageData = message as Message;
                
                Console.WriteLine("recovered {0}", messageData);

                Deliver(DeliveryPath,
                id =>
                {
                    // Do we need to insert the records that were missed when the db server went down here? 
                    //_todoService.AddTodo(messageData + "from big boom", "from big boom");

                    Console.WriteLine("recovered delivery task: {0}, with deliveryId: {1}", messageData.Data, id);

                    if (default(long) == id)
                    {
                        Console.WriteLine("delivery id is empty");
                    }

                    return new Confirmable(id, messageData.Data);
                });
            }
            else if (message is Confirmation)
            {
                var deliveryId = ((Confirmation)message).DeliveryId;
                Console.WriteLine("recovered confirmation of {0}", deliveryId);
                ConfirmDelivery(deliveryId);
            }
            else
                return false;
            return true;
        }

        public override TimeSpan RedeliverInterval
        {
            get { return TimeSpan.FromSeconds(3); }
        }

        //protected override bool ReceiveCommand(object message)
        //{
        //    if (message as string == "boom")
        //        throw new Exception("Controlled devastation");
        //    else if (message is Message)
        //    {
        //        Persist(message as Message, m =>
        //        {
        //            Deliver(DeliveryPath,
        //            id =>
        //            {
        //                Console.WriteLine("sending: {0}, with deliveryId: {1}", m.Data, id);

        //                // INSERT todo into MS SQL Server database
        //                //var canAdd = _todoService.CanAddTodo(m.Data);

        //                //return false;

        //                //if (canAdd)
        //                //{
        //                //    return new Confirmable(id, m.Data);
        //                //}
        //                //else
        //                //{
        //                //    //Self.Tell(new Failure(),Self);
        //                //    throw new Exception("devastation");
        //                //}
        //            });
        //        });
        //    }
        //    else if (message is Confirmation)
        //    {
        //        var m = message as Confirmation;
        //        ConfirmDelivery(m.DeliveryId);
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        protected override bool ReceiveCommand(object message)
        {
            if (message as string == "boom")
                throw new Exception("Controlled devastation");
            else if (message is Message)
            {
                Persist(message as Message, m =>
                {
                    Deliver(DeliveryPath,
                        id =>
                        {
                            Console.WriteLine("sending: {0}, with deliveryId: {1}", m.Data, id);
                            return new Confirmable(id, m.Data);
                        });
                });
            }
            else if (message is Confirmation)
            {
                Persist(message as Confirmation, m => ConfirmDelivery(m.DeliveryId));
            }
            else return false;
            return true;
        }



        //protected override void PreRestart(Exception reason, object message)
        //{
        //    Console.WriteLine("in PreRestart");
        //    Self.Tell(message);
        //}
    }

    public class DeliveryActor : UntypedActor
    {
        private bool Confirming = true;

        protected override void OnReceive(object message)
        {
            if (message as string == "start")
            {
                Confirming = true;
            }
            if (message as string == "stop")
            {
                Confirming = false;
            }
            if (message is Confirmable)
            {
                var msg = message as Confirmable;
                if (Confirming)
                {
                    new TodoServiceBusinessLogic().AddTodo("blah ");
                    Console.WriteLine("Confirming delivery of message id: {0} and data: {1}", msg.DeliveryId, msg.Data);
                    Context.Sender.Tell(new Confirmation(msg.DeliveryId));
                }
                else
                {
                    Console.WriteLine("Ignoring message id: {0} and data: {1}", msg.DeliveryId, msg.Data);
                }
            }
        }
    }
}

