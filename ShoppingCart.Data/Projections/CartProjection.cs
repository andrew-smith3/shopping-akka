using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ShoppingCart.Data.Projections
{
    public class CartProjection : Projection
    {
        public static string ProjectionType = nameof(CartProjection);

        public override string Type => ProjectionType;

        public Guid UserId { get; }

        public List<ProductProjection> Products { get; }

        public double Subtotal => Products.Sum(p => p.Price);

        public CartProjection(Guid userId)
        {
            UserId = userId;
            Products = new List<ProductProjection>();
        }

        [JsonConstructor]
        public CartProjection(Guid id, DateTime created, Guid userId, List<ProductProjection> products) : base(id, created)
        {
            UserId = userId;
            Products = products;
        }
    }
}
