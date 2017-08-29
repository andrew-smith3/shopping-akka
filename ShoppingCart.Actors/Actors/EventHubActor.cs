using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using ShoppingCart.Actors.Actors.Projections;
using ShoppingCart.Data.Events;

namespace ShoppingCart.Actors.Actors
{
    public class EventHubActor : ReceiveActor
    {
        public static Props CreateProps()
        {
            return Props.Create(() => new EventHubActor());
        }

        public EventHubActor()
        {
            Ready();
        }

        public void Ready()
        {
            Receive<ItemAddedToCart>(ev =>
            {
                var childName = nameof(CartProjectionActor) + ev.UserId.ToString();

                var child = Context.Child(childName);

                if (child == ActorRefs.Nobody)
                {
                    var props = CartProjectionActor.CreateProps(ev.UserId);
                    child = Context.ActorOf(props, childName);
                }
            });
        }
    }
}
