using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akka.Actor;
using Akka.Util.Internal;
using Newtonsoft.Json;
using ShoppingCart.Data.Events;
using ShoppingCart.Data.Projections;
using ShoppingCart.Data.ProjectionStore;

namespace ShoppingCart.Actors.Actors.Projections
{
    public class InventoryProjectionActor : ReceiveActor
    {

        public static Props CreateProps()
        {
            return Props.Create(() => new InventoryProjectionActor());
        }

        public static IEnumerable<Type> Events()
        {
            yield return typeof(NewProductAddedToInventory);
            yield return typeof(ProductRestocked);
        }

        private SqliteProjectionStore _projectionStore;
        private InventoryProjection _projection;

        public InventoryProjectionActor()
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
