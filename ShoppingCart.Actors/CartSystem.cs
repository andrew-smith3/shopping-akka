using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
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
            _system = ActorSystem.Create("CartActorSystem");

            var consoleWriterActor = _system.ActorOf(ConsoleWriterActor.CreateProps());
            _cartCoordinatorActor = _system.ActorOf(CartCoordinatorActor.CreateProps(consoleWriterActor));
        }

        public void AddItemToCart(Guid userId, Product product)
        {
            var command = new AddItemToCartCommand(userId, product);

            _cartCoordinatorActor.Tell(command);
        }
    }
}
