using System;
using System.Linq;
using Akka.Actor;
using Newtonsoft.Json;
using ShoppingCart.Data.Events;
using ShoppingCart.Data.Persistence;
using ShoppingCart.Data.Projections;
using ShoppingCart.Data.Queries.Cart;
using ShoppingCart.Data.Queries.Inventory;
using ShoppingCart.Data.ReadModels;

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
        private readonly SqliteStore _store = new SqliteStore();
        private CartReadModel _cartReadModel;

        public CartQueryActor(Guid userId, IActorRef inventoryQueryActor)
        {
            _userId = userId;
            _inventoryQueryActor = inventoryQueryActor;

            InitializeProjection();

            Ready();
        }

        private void InitializeProjection()
        {
            var data = _store.Retrieve<CartReadModel>(_userId);
            if (data == null)
            {
                _cartReadModel = new CartReadModel(_userId);
            }
        }

        private void Ready()
        {
            Receive<CartQuery>(q =>
            {
                Sender.Tell(_cartReadModel);
            });

            ReceiveAsync<ProductAddedToCart>(async e =>
            {
                var product = await _inventoryQueryActor.Ask<ProductReadModel>(new ProductQuery(e.ProductId));
                _cartReadModel.Products.Add(product);
                _store.Save(_cartReadModel);
            });

            Receive<ProductRemovedFromCart>(e =>
            {
                var product = _cartReadModel.Products.First(p => p.Id == e.ProductId);
                _cartReadModel.Products.Remove(product);
                _store.Save(_cartReadModel);
            });
        }
    }
}
