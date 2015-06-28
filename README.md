# TodoReference

.NET Todo Akka Reference Application using http://getakka.net/

Akka Remoting Sample that includes: 

-- Todo Console Application (TodoSystem console application)

-- Todo Web Application (TodoWebApplication web api/mvc web project)

-- Todo Data Access (TodoDataAccess class libary)

-- Todo Business Logic Service (TodoService class libary)

-- Todo Data Model - common messages (TodoDataModel class libary)

-- Todo Actors - common actors shared between Console Application and Web Application (TodoActors class libary)

-- Todo Actor Service (TodoActorService class libary)

Actors
- TodoActor - Persistent Actor that is At least once delivery
- TodoCoordinatorActor and TodoChildActor - Supervisor Strategry Pattern

Todo
- An actor that can handle back pressure.
- A DB Communicator actor with db up/down states. Won't send to databaes unless db is in an up state. - http://stackoverflow.com/questions/28387589/how-to-handle-exceptions-within-the-actor-in-akka-net

Features

- Local only actors 
- Remoting
- Clustering
- Hocon configuration from app.config