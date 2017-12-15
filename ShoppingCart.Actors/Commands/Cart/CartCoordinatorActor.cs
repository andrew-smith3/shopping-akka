using System;
using System.Collections.Generic;
using Akka.Actor;
using ShoppingCart.Data.Commands;
using ShoppingCart.Data.Commands.Cart;

namespace ShoppingCart.Actors.Commands.Cart
{
    public class CartCoordinatorActor : ReceiveActor
    {

        public static Props CreateProps(IActorRef queryActor, IActorRef consoleWriterActor)
        {
            return Props.Create(() => new CartCoordinatorActor(queryActor, consoleWriterActor));
        }

        public static IEnumerable<Type> AcceptedCommands()
        {
            yield return typeof(AddProductToCartCommand);
            yield return typeof(RemoveProductFromCartCommand);
        }

        private readonly IActorRef _queryActor;
        private readonly IActorRef _consoleWriterActor;

        public CartCoordinatorActor(IActorRef queryActor, IActorRef consoleWriterActor)
        {
            _queryActor = queryActor;
            _consoleWriterActor = consoleWriterActor;
            Ready();
        }

        private void Ready()
        {
            Receive<AddProductToCartCommand>(c =>
            {
                ForwardToCartActor(c, c.UserId);
            });

            Receive<RemoveProductFromCartCommand>(c =>
            {
                ForwardToCartActor(c, c.UserId);
            });
        }

        private void ForwardToCartActor(Command command, Guid userId)
        {
            var childName = CartActor.GetName(userId);

            var child = Context.Child(childName);

            if (child == ActorRefs.Nobody)
            {
                var props = CartActor.CreateProps(userId, _queryActor, _consoleWriterActor);
                child = Context.ActorOf(props, childName);
            }
            child.Forward(command);
        }
    }
}
