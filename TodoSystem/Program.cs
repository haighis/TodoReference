//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2015 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2015 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Configuration;
using System.Security.AccessControl;
using System.Threading;
using Akka.Actor;
using Akka.Cluster.Routing;
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

        //private static ActorSystem system;
        private static IActorRef todoCoordinator;

        /// <summary>
        /// Send to Backend. - Sample only. You wouldn't do this in production. Here an actor system is created each time
        /// this method is called which is an expensive operation.
        /// </summary>
        private static void SendToBackend()
        {
            var config =
                    ConfigurationFactory.ParseString("akka.remote.helios.tcp.port=" + 0)
                        .WithFallback(_clusterConfig);

            //var todoCoordinator = system.ActorOf(Props.Create(() => new TodoCoordinatorActor()), "todocoordinator");
            //var todoCoordinator = system.ActorSelection(ActorPaths.CoordinatorPath);
            
            //var todoCoordinatorRouter = system.ActorOf(Props.Create(() => new TodoCoordinatorActor()).WithRouter(FromConfig.Instance), "todogroup");
            //Console.WriteLine("path " + todoCoordinatorRouter.Path);

            if (todoCoordinator != null)
            {
                todoCoordinator.Tell(new Message("test", Guid.NewGuid()));
            }
        }

        private static void LaunchBackend(string[] args)
        {
            var port = args.Length > 0 ? args[0] : "0";

            var config =
                ConfigurationFactory.ParseString("akka.remote.helios.tcp.port=" + port).WithFallback(_clusterConfig);

            
            var    system = ActorSystem.Create("todosystem", config);
            

            //var backendRouter =
            //    system.ActorOf(
            //        Props.Empty.WithRouter(
            //        new ClusterRouterGroup(new ConsistentHashingGroup("/user/backend"),
            //            new ClusterRouterGroupSettings(10, false, "backend", ImmutableHashSet.Create("/user/backend"))
            //                                )
            //                               )
            //            );

            //Props prop = new Props(new TodoCoordinatorActor());

            // Create Coordinator Actor that will supervise risky child (Character Actor) actor's
            //var actor = system.ActorOf(Props.Create( () => new TodoCoordinatorActor()), "todocoordinator");

            //if (todoCoordinator == null)
            //{
            //    todoCoordinator = system.ActorOf(Props.Create(() => new TodoCoordinatorActor()).WithRouter(
            //    new ClusterRouterGroup(new ConsistentHashingGroup("/user/todocoordinator"),
            //            new ClusterRouterGroupSettings(10, true, "program", ImmutableHashSet.Create("/user/todocoordinator"))
            //                                )), "todocoordinator");
            //}
            
            //Console.WriteLine("path " + actor.Path);

            if (todoCoordinator == null)
            {
                todoCoordinator = system.ActorOf(Props.Create(() => new TodoCoordinatorActor()).WithRouter(FromConfig.Instance), "todogroup");    
            }
        }
    }
}

