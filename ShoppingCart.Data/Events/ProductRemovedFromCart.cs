using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ShoppingCart.Data.Events
{
    public class ProductRemovedFromCart : Event
    {
        public Guid UserId { get; }

        public Guid ProductId { get; }

        public ProductRemovedFromCart(Guid userId, Guid productId) 
            : this(Guid.NewGuid(), DateTime.Now, userId, productId)
        {
        }

        [JsonConstructor]
        public ProductRemovedFromCart(Guid id, DateTime date, Guid userId, Guid productId) : base(id, date)
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
