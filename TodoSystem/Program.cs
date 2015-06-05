﻿//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2015 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2015 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using Akka.Actor;
using Akka.Configuration;

namespace System2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(@"
akka {  
    log-config-on-start = on
    stdout-loglevel = DEBUG
    loglevel = DEBUG
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
        
        debug {  
          receive = on 
          autoreceive = on
          lifecycle = on
          event-stream = on
          unhandled = on
        }
    }
    remote {
        helios.tcp {
		    port = 8090
		    hostname = localhost
        }
    }
}
");
            //testing connectivity
            using (var system = ActorSystem.Create("system2", config))
            {
                //get a reference to the remote actor
                var test = system
                    .ActorSelection("akka.tcp://system1@localhost:8080/user/someActor");
                //send a message to the remote actor
                test.Tell("test from remote ()()()()()()()()()()()()()()()()()()()");

                Console.ReadLine();
            }
        }
    }
}
