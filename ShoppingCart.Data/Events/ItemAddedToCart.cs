using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ShoppingCart.Data.Models;

namespace ShoppingCart.Data.Events
{
    public class ItemAddedToCart : Event
    {
        public Guid UserId { get; }

        public Product Product { get; }

        public ItemAddedToCart(Guid userId, Product product) 
            : this(Guid.NewGuid(), DateTime.Now, userId, product)
        {
        }

        [JsonConstructor]
        public ItemAddedToCart(Guid id, DateTime date, Guid userId, Product product) : base(id, date)
        {
            UserId = userId;
            Product = product;
        }

        public override string ToString()
        {
            return $"{Product.Name} added to {UserId} cart";
        }
    }
}
