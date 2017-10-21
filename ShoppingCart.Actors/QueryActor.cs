using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Util.Internal;
using ShoppingCart.Actors.Query;
using ShoppingCart.Actors.Query.Cart;

namespace ShoppingCart.Actors
{
    public class QueryActor : ReceiveActor
    {
        public static Props CreateProps()
        {
            return Props.Create(() => new QueryActor());
        }

        private readonly Dictionary<Type, IActorRef> _queryRegistry = new Dictionary<Type, IActorRef>();

        public QueryActor()
        {
            var inventoryQueryActor = Context.ActorOf(InventoryQueryActor.CreateProps());
            RegisterActor(inventoryQueryActor, InventoryQueryActor.AcceptedQueries());

            var cartQueryCoordinatorActor = Context.ActorOf(CartQueryCoordinatorActor.CreateProps(inventoryQueryActor));
            RegisterActor(cartQueryCoordinatorActor, CartQueryCoordinatorActor.AcceptedQueries());
            Ready();
        }

        private void RegisterActor(IActorRef actor, IEnumerable<Type> acceptedQueries)
        {
            acceptedQueries.ForEach(query =>
            {
                _queryRegistry.Add(query, actor);
            });
        }

        private void Ready()
        {
            Receive<Data.Queries.Query>(q =>
            {
                if (_queryRegistry.ContainsKey(q.GetType()))
                {
                    _queryRegistry[q.GetType()].Forward(q);
                }
                else
                {
                    Sender.Tell("No actor registered to accept command.");
                }
            });
        }
    }
}
