using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Util.Internal;
using Newtonsoft.Json;
using ShoppingCart.Data.Events;
using ShoppingCart.Data.Persistence;
using ShoppingCart.Data.Queries.Inventory;
using ShoppingCart.Data.ReadModels;

namespace ShoppingCart.Actors.Query
{
    public class InventoryQueryActor : ReceiveActor
    {
        public static Props CreateProps()
        {
            return Props.Create(() => new InventoryQueryActor());
        }

        public static IEnumerable<Type> AcceptedQueries()
        {
            yield return typeof(ProductStockStatusQuery);
            yield return typeof(ProductQuery);
            yield return typeof(InventoryQuery);
        }

        public static IEnumerable<Type> Events()
        {
            yield return typeof(NewProductAddedToInventory);
            yield return typeof(ProductRestocked);
        }

        private readonly SqliteStore _store;
        private InventoryReadModel _readModel;

        public InventoryQueryActor()
        {
            _store = new SqliteStore();
            InitializeProjection();
            Events().ForEach(eventType =>
            {
                Context.System.EventStream.Subscribe(Self, eventType);
            });
            Ready();
        }

        private void InitializeProjection()
        {
            var readModel = _store.Retrieve<InventoryReadModel>(Guid.Empty);
            if (readModel == null)
            {
                _readModel = new InventoryReadModel(Guid.Empty);
            }
        }

        private void Ready()
        {
            SetupQueryHandlers();
            SetupEventHandlers();
        }

        private void SetupQueryHandlers()
        {
            Receive<InventoryQuery>(q =>
            {
                Sender.Tell(_readModel);
            });

            Receive<ProductStockStatusQuery>(q =>
            {
                var inStock = _readModel.Products.Any(p => p.Id == q.ProductId);
                Sender.Tell(inStock);
            });

            Receive<ProductQuery>(q =>
            {
                var product = _readModel.Products.First(p => p.Id == q.ProductId);
                Sender.Tell(product);
            });
        }

        private void SetupEventHandlers()
        {
            Receive<NewProductAddedToInventory>(e =>
            {
                var product = new ProductReadModel(e.Id, e.Name, e.Price, e.Stock);

                _readModel.Products.Add(product);
                Persist();
            });

            Receive<ProductRestocked>(e =>
            {
                var product = _readModel.Products.First(p => p.Id == e.ProductId);
                product.AddStock(e.AmountAdded);
                Persist();
            });
        }

        private void Persist()
        {
            _store.Save(_readModel);
        }
    }
}
