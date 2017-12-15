using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Util.Internal;
using ShoppingCart.Data.Events;
using ShoppingCart.Data.Queries.Cart;

namespace ShoppingCart.Actors.Query.Cart
{
    public class CartQueryCoordinatorActor : ReceiveActor
    {
        private static IEnumerable<Type> GetEventTypes()
        {
            yield return typeof(ProductAddedToCart);
            yield return typeof(ProductRemovedFromCart);
        }

        public static Props CreateProps(IActorRef inventoryActor)
        {
            return Props.Create(() => new CartQueryCoordinatorActor(inventoryActor));
        }

        public static IEnumerable<Type> AcceptedQueries()
        {
            yield return typeof(CartQuery);
        }

        private readonly IActorRef _inventoryActor;

        public CartQueryCoordinatorActor(IActorRef inventoryActor)
        {
            _inventoryActor = inventoryActor;

            GetEventTypes().ForEach(type =>
            {
                Context.System.EventStream.Subscribe(Self, type);
            });
            Ready();
        }

        private void Ready()
        {
            Receive<CartQuery>(q =>
            {
                ForwardToChild(q.UserId, q);
            });
        
            Receive<ProductAddedToCart>(e =>
            {
                ForwardToChild(e.UserId, e);
            });

            Receive<ProductRemovedFromCart>(e =>
            {
                ForwardToChild(e.UserId, e);
            });
        }

        private void ForwardToChild(Guid userId, object message)
        {
            var childName = CartQueryActor.GetName(userId);
            var child = Context.Child(childName);
            if (child == ActorRefs.Nobody)
            {
                var props = CartQueryActor.CreateProps(userId, _inventoryActor);
                child = Context.ActorOf(props, childName);
            }
            child.Forward(message);
        }
    }
}
