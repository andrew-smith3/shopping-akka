using System;
using Akka.Actor;
using Akka.Persistence;
using Newtonsoft.Json;
using ShoppingCart.Data.Commands.Cart;
using ShoppingCart.Data.Events;
using ShoppingCart.Data.Queries.Inventory;

namespace ShoppingCart.Actors.Commands.Cart
{
    public class CartActor : ReceivePersistentActor
    {
        public static Props CreateProps(Guid userId, IActorRef queryActor, IActorRef consoleWriterActor)
        {
            return Props.Create(() => new CartActor(userId, queryActor, consoleWriterActor));
        }

        public static string GetName(Guid userId)
        {
            return $"CartActor{userId}";
        }

        public override string PersistenceId => GetName(_userId); 

        private readonly Guid _userId;
        private readonly IActorRef _queryActor;
        private readonly IActorRef _consoleWriterActor;
        private Domain.Cart _cart;

        public CartActor(Guid userId, IActorRef queryActor, IActorRef consoleWriterActor)
        {
            _userId = userId;
            _queryActor = queryActor;
            _consoleWriterActor = consoleWriterActor;

            _cart = new Domain.Cart(_userId);
            Ready();
        }

        private void Ready()
        {
            Recover<ItemAddedToCart>(ev =>
            {
                _cart.AddProductToCart(ev.ProductId);
            });

            Recover<SnapshotOffer>(offer =>
            {
                var json = offer.Snapshot as string;
                if (json != null)
                {
                    _cart = JsonConvert.DeserializeObject<Domain.Cart>(json);
                }
            });

            CommandAsync<AddProductToCartCommand>(async command =>
            {
                var stockStatus = await _queryActor.Ask<bool>(new ProductStockStatusQuery(command.ProductId));
                if (stockStatus == false)
                {
                    Sender.Tell("Out of stock");
                }
                try
                {
                    _cart.AddProductToCart(command.ProductId);
                }
                catch (Exception e)
                {
                    var result = CommandResult.Error(e.Message);
                    Sender.Tell(result);
                    return;
                }
                PersistEventAndSnapshot(new ItemAddedToCart(_userId, command.ProductId));
                Sender.Tell(CommandResult.Success());
            });

            Command<SaveSnapshotSuccess>(x =>
            {
                _consoleWriterActor.Tell($"Snapshot saved cart for user {_userId}");
            });

            Command<SaveSnapshotFailure>(x =>
            {
                _consoleWriterActor.Tell($"Snapshot failed to save cart for user {_userId}");
            });
        }

        private void PersistEventAndSnapshot(Event eventToPersist)
        {
            Persist(eventToPersist, persistedEvent =>
            {
                if (LastSequenceNr % 3 == 0)
                {
                    SaveSnapshot(JsonConvert.SerializeObject(_cart));
                }
                Context.System.EventStream.Publish(persistedEvent);
            });
        }
    }
}
