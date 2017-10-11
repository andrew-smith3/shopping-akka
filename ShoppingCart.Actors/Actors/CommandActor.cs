using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Util.Internal;
using ShoppingCart.Data.Commands;
using ShoppingCart.Actors.Actors.Commands;

namespace ShoppingCart.Actors.Actors
{
    public class CommandActor : ReceiveActor
    {
        public static Props CreateProps()
        {
            return Props.Create(() => new CommandActor());
        }

        private readonly Dictionary<Type, IActorRef> _registry = new Dictionary<Type, IActorRef>();

        public CommandActor()
        {
            var consoleWriterActor = Context.ActorOf(ConsoleWriterActor.CreateProps());

            var cartCoordinatorActor = Context.ActorOf(CartCoordinatorActor.CreateProps(consoleWriterActor));
            RegisterActor(cartCoordinatorActor, CartCoordinatorActor.AcceptedCommands());

            var inventoryActor = Context.ActorOf(InventoryActor.CreateProps());
            RegisterActor(inventoryActor, InventoryActor.AcceptedCommands());

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
    }
}
