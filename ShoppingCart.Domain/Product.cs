using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Domain
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
