using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Util.Internal;
using Newtonsoft.Json;
using ShoppingCart.Data.Events;
using ShoppingCart.Data.Projections;
using ShoppingCart.Data.ProjectionStore;
using ShoppingCart.Data.Queries.Inventory;

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
        }

        public static IEnumerable<Type> Events()
        {
            yield return typeof(NewProductAddedToInventory);
            yield return typeof(ProductRestocked);
        }

        private readonly SqliteProjectionStore _projectionStore;
        private InventoryProjection _projection;

        public InventoryQueryActor()
        {
            _projectionStore = new SqliteProjectionStore();
            InitializeProjection();
            Events().ForEach(eventType =>
            {
                Context.System.EventStream.Subscribe(Self, eventType);
            });
            Ready();
        }

        private void InitializeProjection()
        {
            var projectionData = _projectionStore.Retrieve(Guid.Empty, InventoryProjection.ProjectionType);
            if (string.IsNullOrEmpty(projectionData))
            {
                _projection = new InventoryProjection();
            }
            else
            {
                _projection = JsonConvert.DeserializeObject<InventoryProjection>(projectionData);
            }
        }

        private void Ready()
        {
            SetupQueryHandlers();
            SetupEventHandlers();
        }

        private void SetupQueryHandlers()
        {
            Receive<ProductStockStatusQuery>(q =>
            {
                var inStock = _projection.Inventory.Any(p => p.Id == q.ProductId);
                Sender.Tell(inStock);
            });

            Receive<ProductQuery>(q =>
            {
                var product = _projection.Inventory.First(p => p.Id == q.ProductId);
                Sender.Tell(product);
            });
        }

        private void SetupEventHandlers()
        {
            Receive<NewProductAddedToInventory>(e =>
            {
                var product = new ProductProjection
                {
                    Id = e.Id,
                    Name = e.Name,
                    Stock = e.Stock,
                    Price = e.Price
                };

                _projection.Inventory.Add(product);
                Persist();
            });

            Receive<ProductRestocked>(e =>
            {
                var product = _projection.Inventory.First(p => p.Id == e.ProductId);
                product.Stock += e.AmountAdded;
                Persist();
            });
        }

        private void Persist()
        {
            _projectionStore.Store(Guid.Empty, InventoryProjection.ProjectionType, JsonConvert.SerializeObject(_projection));
        }
    }
}
