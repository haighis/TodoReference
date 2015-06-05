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

    public class Message
    {
        public Message(string data)
        {
            this.Data = data;
        }

        public string Data { get; private set; }
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
