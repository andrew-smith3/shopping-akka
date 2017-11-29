using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ShoppingCart.Data.ReadModels;

namespace ShoppingCart.Data.Projections
{
    public class CartReadModel : ReadModel
    {
        public static string ProjectionType = nameof(CartReadModel);

        public Guid UserId { get; }

        public List<ProductReadModel> Products { get; }

        public double Subtotal => Products.Sum(p => p.Price);

        public CartReadModel(Guid userId) : base(userId)
        {
            UserId = userId;
            Products = new List<ProductReadModel>();
        }

        [JsonConstructor]
        public CartReadModel(Guid id, Guid userId, List<ProductReadModel> products) : base(id)
        {
            UserId = userId;
            Products = products;
        }
    }
}
