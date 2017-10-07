using Akka.Actor;
using ShoppingCart.Data.Commands;

namespace ShoppingCart.Actors.Actors
{
    public class CartCoordinatorActor : ReceiveActor
    {

        public static Props CreateProps(IActorRef consoleWriterActor)
        {
            return Props.Create(() => new CartCoordinatorActor(consoleWriterActor));
        }

        private readonly IActorRef _consoleWriterActor;

        public CartCoordinatorActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
            Ready();
        }

        private void Ready()
        {
            Receive<AddItemToCartCommand>(c =>
            {
                var childName = CartActor.GetName(c.UserId);

                var child = Context.Child(childName);

                if (child == ActorRefs.Nobody)
                {
                    var props = CartActor.CreateProps(c.UserId, _consoleWriterActor);
                    child = Context.ActorOf(props, childName);
                }

                child.Tell(c);
            });
        }
    }
}
