using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Models
{
    public class Cart
    {
        public Guid UserId { get; }

        private readonly List<Product> _products;

        public Cart(Guid userId)
        {
            UserId = userId;
            _products = new List<Product>();
        }

        public void AddProductToCart(Product product)
        {
            _products.Add(product);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Cart {UserId} contains {_products.Count} items (\n");
            _products.ForEach( x => sb.Append($"  {x.ToString()},\n"));
            sb.Append(")");
            return sb.ToString();
        }
    }
}
