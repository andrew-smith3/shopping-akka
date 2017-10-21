using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Persistence;
using ShoppingCart.Data.Commands.Inventory;
using ShoppingCart.Data.Events;
using ShoppingCart.Domain;

namespace ShoppingCart.Actors.Commands
{
    public class InventoryActor : ReceivePersistentActor
    {
        public static Props CreateProps()
        {
            return Props.Create(() => new InventoryActor());
        }

        public static IEnumerable<Type> AcceptedCommands()
        {
            yield return typeof(AddNewProductCommand);
            yield return typeof(RestockProductCommand);
        }

        private const int SnapshotInterval = 5;

        public override string PersistenceId => nameof(InventoryActor);

        private readonly Inventory _inventory;

        public InventoryActor()
        {
            _inventory = new Inventory();
            Ready();
        }

        private void Ready()
        {
            Recover<NewProductAddedToInventory>(RecoverNewProductAddedToInventory, null);
            Command<AddNewProductCommand>(AddNewProduct, null);

            Recover<ProductRestocked>(RecoverProductRestocked, null);
            Command<RestockProductCommand>(command =>
            {
                try
                {
                    _inventory.RestockProduct(command.ProductId, command.AmountToAdd);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Restock error");
                    Sender.Tell(e.Message);
                    return;
                }
                var ev = new ProductRestocked(command.ProductId, command.AmountToAdd);
                PersistEventAndSnapshot(ev);
                Sender.Tell("OK");
            });

            Command<SaveSnapshotSuccess>(x =>
            {
                Console.WriteLine($"Snapshot saved for inventory");
            });

            Command<SaveSnapshotFailure>(x =>
            {
                Console.WriteLine($"Snapshot failed to save for inventory");
            });
        }

        private void AddNewProduct(AddNewProductCommand c)
        {
            var product = new Product
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price,
                Stock = c.Stock
            };
            try
            {
                _inventory.AddNewProduct(product);
            }
            catch (Exception e)
            {
                Console.WriteLine("Product not new");
                Sender.Tell(e.Message);
                return;
            }

            var ev = new NewProductAddedToInventory(c.Id, c.Name, c.Price, c.Stock);
            PersistEventAndSnapshot(ev);
            Sender.Tell("OK");
        }

        private void RecoverNewProductAddedToInventory(NewProductAddedToInventory e)
        {
            var product = new Product
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price,
                Stock = e.Stock
            };
            _inventory.AddNewProduct(product);
        }

        private void RecoverProductRestocked(ProductRestocked ev)
        {
            _inventory.RestockProduct(ev.ProductId, ev.AmountAdded);
        }

        private void PersistEventAndSnapshot<TEvent>(TEvent domainEvent)
        {
            Persist(domainEvent, ev =>
            {
                if(LastSequenceNr % SnapshotInterval == 0)
                {
                    
                }
                Context.System.EventStream.Publish(ev);
            });
        }

    }
}
