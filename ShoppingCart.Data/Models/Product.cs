using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Models
{
    public class Product
    {
        public string Name { get; set; }

        public int Price { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
