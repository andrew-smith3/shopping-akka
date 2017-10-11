using System;

namespace ShoppingCart.Data.Commands.Inventory
{
    public class AddNewProductCommand : Command
    {
        public Guid Id { get; }

        public string Name { get; }

        public double Price { get; }

        public int Stock { get; }

        public AddNewProductCommand(string name, double price, int stock)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Stock = stock;
        }
    }
}
