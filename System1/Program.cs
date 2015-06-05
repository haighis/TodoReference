//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2015 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2015 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Security.AccessControl;
using Akka.Actor;
using Akka.Configuration;
using TodoActors.Actors;
using TodoDataModel;

namespace System1
{
    /// <summary>
    /// 
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            var system = ActorSystem.Create("system1");
            
            var delivery = system.ActorOf(Props.Create(() => new DeliveryActor()), "delivery");

            var deliverer = system.ActorOf(Props.Create(() => new TodoActor(delivery.Path)));
               
            string input;

            Console.WriteLine("Enter send to send bar or quit to exit.");

            while ((input = Console.ReadLine()) != null)
            {
                var cmd = input;
                switch (cmd)
                {
                    case "quit":
                        return; // Stop the run thread
                    case "send":
                            deliverer.Tell(new Message("bar" + DateTime.Today.ToLongDateString()));

                        break;
                }
            }

            Console.ReadLine();
        }
    }
}

