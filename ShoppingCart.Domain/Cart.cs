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

        public List<Guid> Products { get; }

        public Cart(Guid userId) : this(userId, new List<Guid>())
        {
        }

        [JsonConstructor]
        public Cart(Guid userId, List<Guid> products)
        {
            UserId = userId;
            Products = products;
        }

        public void AddProductToCart(Guid newProductId)
        {
            if (Products.Any(p => newProductId == p))
            {
                throw new Exception("Product already in cart");
            }
            Products.Add(newProductId);
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
