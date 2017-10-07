using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Akka.Util.Internal;
using ShoppingCart.Data.Events;

namespace ShoppingCart.Actors.Actors.Projections
{
    public class CartProjectionCoordinatorActor : ReceiveActor
    {
        private static IEnumerable<Type> GetEventTypes()
        {
            yield return typeof(ItemAddedToCart);
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new CartProjectionCoordinatorActor());
        }

        public CartProjectionCoordinatorActor()
        {
            GetEventTypes().ForEach(type =>
            {
                Context.System.EventStream.Subscribe(Self, type);
            });
            Ready();
        }

        private void Ready()
        {
            Receive<ItemAddedToCart>(e =>
            {
                var childName = CartProjectionActor.GetName(e.UserId);
                var child = Context.Child(childName);
                if (child == ActorRefs.Nobody)
                {
                    var props = CartProjectionActor.CreateProps(e.UserId);
                    child = Context.ActorOf(props, childName);
                }
                child.Tell(e);
            });
        }
    }
}
