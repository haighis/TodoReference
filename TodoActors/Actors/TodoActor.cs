using System;
using System.Linq;
using Akka.Actor;

using Akka.Persistence;
using TodoCommon;
using TodoDataModel;
using TodoService;

namespace TodoActors.Actors
{
    public class TodoActor : AtLeastOnceDeliveryActor
    {
        //TODO move this to be injected to the constructor, dependency injection etc. 
        private ITodoServiceBusinessLogic _todoService = new TodoServiceBusinessLogic();

        public ActorPath DeliveryPath { get; private set; }

        public TodoActor(ActorPath deliveryPath)
        {
            this.DeliveryPath = deliveryPath;
        }

        public override string PersistenceId
        {
            get { return "atleastonce-1"; }
        }

        protected override bool ReceiveRecover(object message)
        {
            if (message is Message)
            {
                var messageData = ((Message)message).Data;
                Console.WriteLine("recovered {0}", messageData);
                Deliver(DeliveryPath,
                id =>
                {
                   // Do we need to insert the records that were missed when the db server went down here? 
                    //_todoService.AddTodo(messageData + "from big boom", "from big boom");
                    Console.WriteLine("recovered delivery task: {0}, with deliveryId: {1}", messageData, id);
                    return new Confirmable(id, messageData);
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
                            
                            // INSERT todo into database 
                            _todoService.AddTodo(m.Data);
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
    }

    public class DeliveryActor : UntypedActor
    {
        bool Confirming = true;

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

