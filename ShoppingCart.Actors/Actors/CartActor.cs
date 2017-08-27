using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Akka.Persistence;
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

        private readonly Cart _cart;

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

            Command<AddItemToCartCommand>(command => Persist(command , c =>
            {
                //should do some validation

                _cart.AddProductToCart(command.Product);

                _consoleWriterActor.Tell(_cart.ToString());
            }));
        }

    }
}
