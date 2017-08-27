using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Akka.Persistence;
using Newtonsoft.Json;
using ShoppingCart.Data.Commands;
using ShoppingCart.Data.Models;

namespace ShoppingCart.Actors.Actors
{
    public class CartActor : ReceivePersistentActor
    {
        public static Props CreateProps(Guid userId, IActorRef consoleWriterActor)
        {
            return Props.Create(() => new CartActor(userId, consoleWriterActor));
        }

        public override string PersistenceId => $"Cart {_userId}"; 

        private readonly Guid _userId;
        private readonly IActorRef _consoleWriterActor;
        private Cart _cart;
        private int _commandsSinceSnapshot = 0;

        public CartActor(Guid userId, IActorRef consoleWriterActor)
        {
            _userId = userId;
            _consoleWriterActor = consoleWriterActor;

            _cart = new Cart(_userId);
            Ready();
        }

        private void Ready()
        {
            Recover<AddItemToCartCommand>(command =>
            {
                _cart.AddProductToCart(command.Product);
            });

            Recover<SnapshotOffer>(offer =>
            {
                var json = offer.Snapshot as string;
                if (json != null)
                {
                    _cart = JsonConvert.DeserializeObject<Cart>(json);
                }
            });

            Command<AddItemToCartCommand>(command => Persist(command , c =>
            {
                //should do some validation

                _cart.AddProductToCart(command.Product);
                if (++_commandsSinceSnapshot % 3 == 0)
                {
                    SaveSnapshot(JsonConvert.SerializeObject(_cart));
                }


                _consoleWriterActor.Tell(_cart.ToString());
            }));

            Command<SaveSnapshotSuccess>(x =>
            {
                _consoleWriterActor.Tell($"Snapshot saved cart for user {_userId}");
            });

            Command<SaveSnapshotFailure>(x =>
            {
                _consoleWriterActor.Tell($"Snapshot failed to save cart for user {_userId}");
            });
        }

    }
}
