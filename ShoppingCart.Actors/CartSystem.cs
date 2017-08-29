using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Akka.Configuration;
using Akka.Persistence.Sqlite;
using ShoppingCart.Actors.Actors;
using ShoppingCart.Data.Commands;
using ShoppingCart.Data.Models;

namespace ShoppingCart.Actors
{
    public class CartSystem
    {
        private readonly ActorSystem _system;
        private readonly IActorRef _cartCoordinatorActor;

        public CartSystem()
        {
            var config = ConfigurationFactory.ParseString(@" 
                akka.persistence.journal.plugin=""akka.persistence.journal.sqlite""
                akka.persistence.journal.sqlite.connection-string=""Data Source=c:\\users\\andrew\\desktop\\akka.db;""
                akka.persistence.journal.sqlite.auto-initialize=true
                akka.persistence.snapshot-store.plugin=""akka.persistence.snapshot-store.sqlite""
                akka.persistence.snapshot-store.sqlite.connection-string=""Data Source=c:\\users\\andrew\\desktop\\akka.db;""
                akka.persistence.snapshot-store.sqlite.auto-initialize=true
            }");

            _system = ActorSystem.Create("CartActorSystem", config);
            SqlitePersistence.Get(_system);

            var consoleWriterActor = _system.ActorOf(ConsoleWriterActor.CreateProps());
            _cartCoordinatorActor = _system.ActorOf(CartCoordinatorActor.CreateProps(consoleWriterActor));
            var eventHub = _system.ActorOf(EventHubActor.CreateProps(), "eventhub");
        }

        public void AddItemToCart(Guid userId, Product product)
        {
            var command = new AddItemToCartCommand(userId, product);

            _cartCoordinatorActor.Tell(command);
        }
    }
}
