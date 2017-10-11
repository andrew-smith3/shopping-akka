using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Projections
{
    public class ProductProjection
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }
    }
}
