using Akka.Actor;
using Akka.Configuration;
using Akka.Persistence.Sqlite;
using ShoppingCart.Data.Commands.Cart;
using ShoppingCart.Data.Commands.Inventory;
using ShoppingCart.Data.Projections;
using ShoppingCart.Data.Queries.Cart;
using System.Threading.Tasks;
using ShoppingCart.Data.Queries.Inventory;
using ShoppingCart.Data.ReadModels;

namespace ShoppingCart.Actors
{
    public class CartSystem
    {
        private readonly ActorSystem _system;
        private readonly IActorRef _commandActor;
        private readonly IActorRef _queryActor;

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

            _queryActor = _system.ActorOf(QueryActor.CreateProps(), "queryActor");
            _commandActor = _system.ActorOf(CommandActor.CreateProps(_queryActor), "commandActor");
        }

        public async Task<CommandResult> AddNewProduct(AddNewProductCommand command)
        {
            return await _commandActor.Ask<CommandResult>(command);
        }

        public async Task<CommandResult> RestockProduct(RestockProductCommand command)
        {
            return await _commandActor.Ask<CommandResult>(command);
        }

        public async Task<CommandResult> AddProductToCart(AddProductToCartCommand command)
        {
            return await _commandActor.Ask<CommandResult>(command);
        }

        public async Task<InventoryReadModel> GetInventory(InventoryQuery query)
        {
            return await _queryActor.Ask<InventoryReadModel>(query);
        }

        public async Task<CartReadModel> GetCart(CartQuery query)
        {
            return await _queryActor.Ask<CartReadModel>(query);
        }
    }
}
