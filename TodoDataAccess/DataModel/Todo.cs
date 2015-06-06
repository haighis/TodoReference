using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoDataAccess.DataModel
{
    public class Todo : IPersistentActor
    {
        public int TodoId { get; set; }
        public string TaskName { get; set; }
        public long DeliveryId { get; set; }
    }

    public interface IPersistentActor
    {
        long DeliveryId { get; set; }
    }
}
