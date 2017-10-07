using Akka.Actor;
using ShoppingCart.Actors.Actors.Projections;

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
            Context.ActorOf(CartProjectionCoordinatorActor.CreateProps());

            
        }
    }
}
