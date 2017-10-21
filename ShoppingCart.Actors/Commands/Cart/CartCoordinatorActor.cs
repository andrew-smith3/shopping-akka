using System;
using System.Collections.Generic;
using Akka.Actor;
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
                var childName = CartActor.GetName(c.UserId);

                var child = Context.Child(childName);

                if (child == ActorRefs.Nobody)
                {
                    var props = CartActor.CreateProps(c.UserId, _queryActor, _consoleWriterActor);
                    child = Context.ActorOf(props, childName);
                }
                child.Forward(c);
            });
        }
    }
}
