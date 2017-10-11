using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Events
{
    public class NewProductAddedToInventory
    {
        public Guid Id { get; }

        public string Name { get; }

        public double Price { get; }

        public int Stock { get; }

        public NewProductAddedToInventory(Guid id, string name, double price, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
        }

    }
}
