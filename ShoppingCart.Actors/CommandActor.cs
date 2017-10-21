using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Util.Internal;
using ShoppingCart.Actors.Commands;
using ShoppingCart.Actors.Commands.Cart;
using ShoppingCart.Data.Commands;

namespace ShoppingCart.Actors
{
    public class CommandActor : ReceiveActor
    {
        public static Props CreateProps(IActorRef queryActor)
        {
            return Props.Create(() => new CommandActor(queryActor));
        }

        private readonly Dictionary<Type, IActorRef> _registry = new Dictionary<Type, IActorRef>();

        public CommandActor(IActorRef queryActor)
        {
            var consoleWriterActor = Context.ActorOf(ConsoleWriterActor.CreateProps());

            var inventoryActor = Context.ActorOf(InventoryActor.CreateProps());
            RegisterActor(inventoryActor, InventoryActor.AcceptedCommands());

            var cartCoordinatorActor = Context.ActorOf(CartCoordinatorActor.CreateProps(queryActor, consoleWriterActor));
            RegisterActor(cartCoordinatorActor, CartCoordinatorActor.AcceptedCommands());

            Ready();
        }

        private void RegisterActor(IActorRef actor, IEnumerable<Type> acceptedCommands)
        {
            acceptedCommands.ForEach(command =>
            {
                _registry.Add(command, actor);
            });
        } 

        public void Ready()
        {
            Receive<Command>(c =>
            {
                if (_registry.ContainsKey(c.GetType()))
                {
                    _registry[c.GetType()].Forward(c);
                }
                else
                {
                    Sender.Tell("No actor registered to accept command.");
                }
            });
        }

        protected override void Unhandled(object message)
        {
            Console.WriteLine($"Message of type {message.GetType()} was unhandled");
        }
    }
}
