using System;
using System.Collections.Generic;
using System.Text;
using ShoppingCart.Data.Models;

namespace ShoppingCart.Data.Events
{
    public class ItemAddedToCart
    {
        public Guid UserId { get; }

        public Product Product { get; }

        public ItemAddedToCart(Guid userId, Product product)
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
