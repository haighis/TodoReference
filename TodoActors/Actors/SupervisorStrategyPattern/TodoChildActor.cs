﻿using Akka.Actor;
using System;
using TodoDataModel;
using TodoService;

namespace TodoActors.Actors.SupervisorStrategyPattern
{
    /// <summary>
    /// Tdo Child Actor for taking on dangerous task of inserting to a database.
    /// </summary>
    public class TodoChildActor : ReceiveActor
    {
        private ITodoServiceBusinessLogic _todoService;
        public TodoChildActor()
        {
            Receive<Message>(msg =>
            {
                _todoService = new TodoServiceBusinessLogic();

                _todoService.AddTodo(msg.Data); 
               
                // Optional Async method to save data

                // Could be changed to a can proceed pattern where we check if the todoo exists and if not then proceed.

                // Call async
                //_todoService.AddTodoAsync(msg.Data).PipeTo(Self);
                //_todoService.AddTodoAsync(msg.Data).ContinueWith(t => { }).PipeTo(Self);
            });  
        }

        /// <summary>
        /// PreRestart method that is called when Actor is restarted. Tell ourself the message so that message
        /// is added to the Mailbox on restart. This will allow the message to be resent when the actor 
        /// restarts.
        /// 
        /// Note this actor message will be lost in the case the actor system is restarted (unplanned restart. I.e. service fails)
        /// 
        /// See for more information: https://petabridge.com/blog/how-actors-recover-from-failure-hierarchy-and-supervision/
        /// See API for more information: http://api.getakka.net/docs/unstable/html/28D639D8.htm
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        protected override void PreRestart(Exception reason, object message)
        {
            Self.Tell(message);
        }

        protected override void PostStop()
        {
            _todoService.Dispose();

            base.PostStop();
        }
    }
}
