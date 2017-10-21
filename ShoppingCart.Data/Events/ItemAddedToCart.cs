using System;
using Newtonsoft.Json;
using ShoppingCart.Domain;

namespace ShoppingCart.Data.Events
{
    public class ItemAddedToCart : Event
    {
        public Guid UserId { get; }

        public Guid ProductId { get; }

        public ItemAddedToCart(Guid userId, Guid productId) 
            : this(Guid.NewGuid(), DateTime.Now, userId, productId)
        {
        }

        [JsonConstructor]
        public ItemAddedToCart(Guid id, DateTime date, Guid userId, Guid productId) : base(id, date)
        {
            UserId = userId;
            ProductId = productId;
        }

        public override string ToString()
        {
            return $"{ProductId} added to {UserId} cart";
        }
    }
}
