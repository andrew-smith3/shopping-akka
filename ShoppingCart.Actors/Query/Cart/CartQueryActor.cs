using System;
using Akka.Actor;
using Newtonsoft.Json;
using ShoppingCart.Data.Events;
using ShoppingCart.Data.Projections;
using ShoppingCart.Data.ProjectionStore;
using ShoppingCart.Data.Queries.Cart;
using ShoppingCart.Data.Queries.Inventory;

namespace ShoppingCart.Actors.Query.Cart
{
    public class CartQueryActor : ReceiveActor
    {
        public static Props CreateProps(Guid userId, IActorRef inventoryQueryActor)
        {
            return Props.Create(() => new CartQueryActor(userId, inventoryQueryActor));
        }

        public static string GetName(Guid userId)
        {
            return $"{nameof(CartQueryActor)}{userId}";
        }

        private readonly Guid _userId;
        private readonly IActorRef _inventoryQueryActor;
        private readonly SqliteProjectionStore _projectionStore = new SqliteProjectionStore();
        private CartProjection _cartProjection;

        public CartQueryActor(Guid userId, IActorRef inventoryQueryActor)
        {
            _userId = userId;
            _inventoryQueryActor = inventoryQueryActor;

            InitializeProjection();

            Ready();
        }

        private void InitializeProjection()
        {
            var data = _projectionStore.Retrieve(_userId, CartProjection.ProjectionType);
            if (data == string.Empty)
            {
                _cartProjection = new CartProjection(_userId);
            }
            else
            {
                _cartProjection = JsonConvert.DeserializeObject<CartProjection>(data);
            }
        }

        private void Ready()
        {
            Receive<CartQuery>(q =>
            {
                Sender.Tell(_cartProjection);
            });

            ReceiveAsync<ItemAddedToCart>(async i =>
            {
                var product = await _inventoryQueryActor.Ask<ProductProjection>(new ProductQuery(i.ProductId));
                _cartProjection.Products.Add(product);
                _projectionStore.Store(_userId, _cartProjection.Type, JsonConvert.SerializeObject(_cartProjection));
            });
        }
    }
}
