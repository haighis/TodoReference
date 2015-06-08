using Akka.Actor;
using System;
using TodoDataModel;
using TodoService;

namespace TodoActors.Actors.SupervisorStrategyPattern
{
    public class TodoChildActor : ReceiveActor
    {
        private ITodoServiceBusinessLogic _todoService;
        public TodoChildActor()
        {
            Receive<Message>(msg =>
            {
                _todoService = new TodoServiceBusinessLogic();

                _todoService.AddTodo(msg.Data); 
               
                // this will be changed to a can proceed pattern where we check if the todoo exists and if not then proceed.

                // Call async
                //_todoService.AddTodoAsync(msg.Data).PipeTo(Self);
                //_todoService.AddTodoAsync(msg.Data).ContinueWith(t => { }).PipeTo(Self);

            });  
        }

        /// <summary>
        /// PreRestart method that is called when Actor is restarted. Tell ourself the message so that message
        /// is added to the Mailbox on restart. This will allow the message to be resent when the actor 
        /// restarts.
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        protected override void PreRestart(Exception reason, object message)
        {
            Console.WriteLine("in PreRestart");
            Self.Tell(message);
        }

        protected override void PostStop()
        {
            _todoService.Dispose();

            base.PostStop();
        }
    }
}
