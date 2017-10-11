using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ShoppingCart.Domain
{
    public class Cart
    {
        public Guid UserId { get; }

        public List<Product> Products { get; }

        public Cart(Guid userId) : this(userId, new List<Product>())
        {
        }

        [JsonConstructor]
        public Cart(Guid userId, List<Product> products)
        {
            UserId = userId;
            Products = products;
        }

        public void AddProductToCart(Product product)
        {
            Products.Add(product);
        }

        public double GetSubtotal()
        {
            return Products.Sum(x => x.Price);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Cart {UserId} contains {Products.Count} items (\n");
            Products.ForEach( x => sb.Append($"  {x.ToString()},\n"));
            sb.Append(")");
            return sb.ToString();
        }
    }
}
