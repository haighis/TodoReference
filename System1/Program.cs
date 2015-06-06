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
using Akka.Persistence.SqlServer;
using TodoActors.Actors;
using TodoDataModel;

namespace System1
{
    internal class Program
    {
        private static void Main()
        {
            // Create Actor system
            var system = ActorSystem.Create("system1");
            
            // Initialize Sql Persistence
            SqlServerPersistence.Init(system);

            // TODO investigate file/memory cache based journal. In our use case the file system/memory (distributed memory cache) will always be available
            // via highly available filesystem (azure cloud service) or distribute memory cache (azure cache)

            // Create Deliver actor
            var delivery = system.ActorOf(Props.Create(() => new DeliveryActor()), "delivery");

            // Create Deliverer actor
            var deliverer = system.ActorOf(Props.Create(() => new TodoActor(delivery.Path)));
               
            string input;

            Console.WriteLine("Enter send to send the message bar or quit to exit.");

            while ((input = Console.ReadLine()) != null)
            {
                var cmd = input;
                switch (cmd)
                {
                    case "quit":
                        return; // Stop the run thread
                    case "send":
                            // Send the message bar to the database
                            deliverer.Tell(new Message("bar" + DateTime.Today.ToLongDateString()));

                        break;
                }
            }

            Console.ReadLine();
        }
    }
}

