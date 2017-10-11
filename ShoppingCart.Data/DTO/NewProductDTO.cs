using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.DTO
{
    public class NewProductDTO
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }
    }
}
