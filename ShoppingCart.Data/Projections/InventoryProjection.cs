using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Projections
{
    public class InventoryProjection : Projection
    {
        public static string ProjectionType = nameof(CartProjection);

        public override string Type => nameof(InventoryProjection);

        public List<ProductProjection> Inventory { get; set; }

        public InventoryProjection()
        {
            Inventory = new List<ProductProjection>();
        }
    }
}
