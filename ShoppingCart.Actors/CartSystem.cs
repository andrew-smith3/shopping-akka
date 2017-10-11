using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Persistence.Sqlite;
using ShoppingCart.Actors.Actors;
using ShoppingCart.Data.Commands;
using ShoppingCart.Data.Commands.Inventory;

namespace ShoppingCart.Actors
{
    public class CartSystem
    {
        private readonly ActorSystem _system;
        private readonly IActorRef _commandActor;
        private readonly IActorRef _eventHubActor;

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

            _commandActor = _system.ActorOf(CommandActor.CreateProps(), "commandActor");
            _eventHubActor = _system.ActorOf(EventHubActor.CreateProps(), "eventhub");
        }

        public async Task<string> AddNewProduct(AddNewProductCommand command)
        {
            return await _commandActor.Ask<string>(command);
        }

        public async Task<string> RestockProduct(RestockProductCommand command)
        {
            return await _commandActor.Ask<string>(command);
        }
    }
}
