using System;
using System.Collections.Generic;
using System.Text;
using ShoppingCart.Data.Models;

namespace ShoppingCart.Data.Projections
{
    public class CartProjection : Projection
    {
        public override string Type => "CartProjection";

        public List<Product> Products { get; }

        public int Subtotal { get; }

        public CartProjection(Guid userId, List<Product> products, int subtotal) : base(userId)
        {
            Products = products;
            Subtotal = subtotal;
        }
    }
}
