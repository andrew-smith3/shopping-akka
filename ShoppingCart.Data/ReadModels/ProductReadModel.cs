using System;

namespace ShoppingCart.Data.ReadModels
{
    public class ProductReadModel
    {
        public Guid Id { get; }

        public string Name { get; }

        public double Price { get; }

        public int Stock { get; private set; }

        public ProductReadModel(Guid id, string name, double price, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
        }

        public void AddStock(int additionalStock)
        {
            Stock += additionalStock;
        }
    }
}
