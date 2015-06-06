using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TodoTests
{
    [TestClass]
    public class TodoActorsTests
    {
        [TestMethod]
        public void TodoActors_can_deliver_message_delivery_confirmation()
        {
            // fail until written
            Assert.IsNotNull(null);
        }

        [TestMethod]
        public void TodoActors_can_deliver_message_to_datastoreunavailable_available()
        {
            // Simulate a data store error. I.e. start the actor system with correct data store sql connection string
            // change data store sql connection string. TODO update 
            // fail until written
            Assert.IsNotNull(null);
        }

        [TestMethod]
        public void TodoActors_can_restart_actor_and_get_messages_from_recover()
        {
            // fail until written
            Assert.IsNotNull(null);
        }
    }
}
