//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2015 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2015 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Configuration;
using System.Security.AccessControl;
using System.Threading;
using Akka.Actor;
using Akka.Configuration;
using Akka.Configuration.Hocon;
using Akka.Persistence.SqlServer;
using Akka.Routing;
using TodoActors.Actors;
using TodoDataModel;
using TodoCommon;

namespace System1
{
    internal class Program
    {
        //private static void Main()
        //{
        //   // #region Persistence
        //   ////  Create Actor system
        //   // var system = ActorSystem.Create("system1");
            
        //   // // Initialize Sql Persistence
        //   // SqlServerPersistence.Init(system);

        //   // // TODO investigate file/memory cache based journal. In our use case the file system/memory (distributed memory cache) will always be available
        //   // // via highly available filesystem (azure cloud service) or distribute memory cache (azure cache)

        //   // // Create Deliver actor
        //   // var delivery = system.ActorOf(Props.Create(() => new DeliveryActor()), "delivery");

        //   // // Create Deliverer actor
        //   // var deliverer = system.ActorOf(Props.Create(() => new TodoActor(delivery.Path)));
               
        //   // string input;

        //   // Console.WriteLine("Enter send to send the message bar or quit to exit.");

        //   // while ((input = Console.ReadLine()) != null)
        //   // {
        //   //     var cmd = input;
        //   //     switch (cmd)
        //   //     {
        //   //         case "quit":
        //   //             return; // Stop the run thread
        //   //         case "boom":
        //   //             deliverer.Tell("boom");
        //   //             break;
        //   //         case "start":
        //   //             delivery.Tell("start");
        //   //             break;
        //   //         case "stop":
        //   //             delivery.Tell("stop");
        //   //             break;
        //   //         case "send":
        //   //                 // Send the message bar to database
        //   //                 deliverer.Tell(new Message("bar - " + DateTime.Now.ToString("g")));
        //   //             break;
        //   //     }
        //   // }

        //   // #endregion

        //    #region Supervisor Strategy Pattern
        //    // Remote or Local. For Cluster see below.
        //    var system = ActorSystem.Create("system1");

        //    // Create Coordinator Actor that will supervise risky child (Character Actor) actor's
        //    var todoCoordinator = system.ActorOf(Props.Create(() => new TodoCoordinatorActor()), "todocoordinator");

        //    string input;

        //    Console.WriteLine("Enter send to send the message bar or quit to exit." + todoCoordinator.Path);

        //    while ((input = Console.ReadLine()) != null)
        //    {
        //        var cmd = input;
        //        switch (cmd)
        //        {
        //            case "quit":
        //                return; // Stop the run thread
        //            case "send":
        //                // Send the message bar to database
        //                todoCoordinator.Tell(new Message("bar - " + DateTime.Now.ToString("g")));

        //                break;
        //        }
        //    }

        //    #endregion

        //    Console.ReadLine();
        //}

        private static void Main(string[] args)
        {
            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");
            _clusterConfig = section.AkkaConfig;
            LaunchBackend(new[] { "2551" });
            LaunchBackend(new[] { "2552" });
            LaunchBackend(new string[0]);

            string input;

            Console.WriteLine("Enter send to send the message bar or quit to exit.");// + todoCoordinator.Path

            while ((input = Console.ReadLine()) != null)
            {
                var cmd = input;
                switch (cmd)
                {
                    case "quit":
                        return; // Stop the run thread
                    case "send":
                        // Send the message bar to database
                        //todoCoordinator.Tell(new Message("bar - " + DateTime.Now.ToString("g")));
                        SendToBackend();

                        break;
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static Config _clusterConfig;

        /// <summary>
        /// Send to Backend. - Sample only. You wouldn't do this in production. Here an actor system is created each time
        /// this method is called which is an expensive operation.
        /// </summary>
        private static void SendToBackend()
        {
            var config =
                    ConfigurationFactory.ParseString("akka.remote.helios.tcp.port=" + 0)
                        .WithFallback(_clusterConfig);
            var system = ActorSystem.Create("todosystem", config);

            var todoCoordinator = system.ActorOf(Props.Create(() => new TodoCoordinatorActor()), "todocoordinator");
            //var todoCoordinator = system.ActorSelection(ActorPaths.CoordinatorPath);
            
            //var todoCoordinator = system.ActorOf(Props.Create(() => new TodoCoordinatorActor()).WithRouter(FromConfig.Instance), "todo");
            Console.WriteLine("path " + todoCoordinator.Path);

            todoCoordinator.Tell(new Message("test"));
        }

        private static void LaunchBackend(string[] args)
        {
            var port = args.Length > 0 ? args[0] : "0";

            var config =
                ConfigurationFactory.ParseString("akka.remote.helios.tcp.port=" + port).WithFallback(_clusterConfig);

            var system = ActorSystem.Create("todosystem",config);

            //Props prop = new Props(new TodoCoordinatorActor());

            // Create Coordinator Actor that will supervise risky child (Character Actor) actor's
            var actor = system.ActorOf(Props.Create(() => new TodoCoordinatorActor()), "todocoordinator");
            Console.WriteLine("path " + actor.Path);
        }
    }
}

