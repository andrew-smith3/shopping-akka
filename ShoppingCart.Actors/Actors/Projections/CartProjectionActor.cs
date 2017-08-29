using System;
using Akka.Actor;
using Akka.Persistence.Query;
using Akka.Persistence.Query.Sql;
using Akka.Streams;
using ShoppingCart.Data.Events;

namespace ShoppingCart.Actors.Actors.Projections
{
    public class CartProjectionActor : ReceiveActor
    {
        public static Props CreateProps(Guid userId)
        {
            return Props.Create(() => new CartProjectionActor(userId));
        }

        public CartProjectionActor(Guid userId)
        {
            var readJournal = PersistenceQuery.Get(Context.System)
                .ReadJournalFor<SqlReadJournal>(SqlReadJournal.Identifier);

            var mat = ActorMaterializer.Create(Context.System);
            var source = readJournal.EventsByPersistenceId($"Cart {userId}", 0L, long.MaxValue);
            source.RunForeach(envelope =>
            {
                Console.WriteLine($"Projection: {envelope.Event.ToString()}");
            }, mat);
        }

        public void Ready()
        {
            Receive<CartUpdated>(cu =>
            {

            });
        }
    }
}
