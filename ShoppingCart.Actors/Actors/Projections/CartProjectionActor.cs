using System;
using System.Collections.Generic;
using Akka.Actor;
using Newtonsoft.Json;
using ShoppingCart.Data.Events;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.Projections;
using ShoppingCart.Data.ProjectionStore;

namespace ShoppingCart.Actors.Actors.Projections
{
    public class CartProjectionActor : ReceiveActor
    {
        public static Props CreateProps(Guid userId)
        {
            return Props.Create(() => new CartProjectionActor(userId));
        }

        public static string GetName(Guid userId)
        {
            return $"{nameof(CartProjectionActor)}{userId}";
        }

        private readonly Guid _userId;
        private readonly SqliteProjectionStore _projectionStore = new SqliteProjectionStore();
        private CartProjection _cartProjection;

        public CartProjectionActor(Guid userId)
        {
            _userId = userId;

            InitializeProjection();

            Ready();
        }

        private void InitializeProjection()
        {
            var data = _projectionStore.Retrieve(_userId, CartProjection.ProjectionType);
            if (data == string.Empty)
            {
                _cartProjection = new CartProjection(_userId, new List<Product>(), 0);
            }
            else
            {
                _cartProjection = JsonConvert.DeserializeObject<CartProjection>(data);
            }
        }

        private void Ready()
        {
            Receive<ItemAddedToCart>(i =>
            {
                _cartProjection.Products.Add(i.Product);

                _projectionStore.Store(_userId, _cartProjection.Type, JsonConvert.SerializeObject(_cartProjection));
            });
        }
    }
}
