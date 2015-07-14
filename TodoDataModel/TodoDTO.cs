using System;
using Akka.Routing;

namespace TodoDataModel
{
    public class TodoDto
    {
        public TodoDto(string taskName, string desc)
        {
            TaskName = taskName;
            Description = desc;
        }

        public string TaskName { get; set; }

        public string Description { get; set; }
    }

    public class Message : IConsistentHashable
    {
        public Message(string data, Guid guid)
        {
            this.Data = data;
            Identifier = guid;
        }

        public Guid Identifier { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Identifier, Data);
        }

        public string Data { get; private set; }

        public object ConsistentHashKey 
        {
            get { return Identifier; } 
        }
    }

    public class Confirmable
    {
        public Confirmable(long deliveryId, string data)
        {
            this.DeliveryId = deliveryId;
            this.Data = data;
        }

        public long DeliveryId { get; private set; }

        public string Data { get; private set; }
    }
    public class Confirmation
    {
        public Confirmation(long deliveryId)
        {
            this.DeliveryId = deliveryId;
        }

        public long DeliveryId { get; private set; }
    }
}
