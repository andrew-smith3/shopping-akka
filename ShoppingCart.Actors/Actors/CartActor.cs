using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using ShoppingCart.Data.Commands;
using ShoppingCart.Data.Models;

namespace ShoppingCart.Actors.Actors
{
    public class CartActor : ReceiveActor
    {
        public static Props CreateProps(Guid userId, IActorRef consoleWriterActor)
        {
            return Props.Create(() => new CartActor(userId, consoleWriterActor));
        }

        private Guid _userId;

        private readonly IActorRef _consoleWriterActor;

        private readonly Cart _cart;

        public CartActor(Guid userId, IActorRef consoleWriterActor)
        {
            _userId = userId;
            _consoleWriterActor = consoleWriterActor;

            _cart = new Cart();
            Ready();
        }

        private void Ready()
        {
            Receive<AddItemToCartCommand>(command =>
            {
                //should do some validation

                _cart.AddProductToCart(command.Product);

                _consoleWriterActor.Tell(_cart.ToString());
            });
        }
    }
}
