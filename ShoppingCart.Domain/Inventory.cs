using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain
{
    public class Inventory
    {

        private IEnumerable<Product> Products => _products;

        private readonly List<Product> _products = new List<Product>();

        public Inventory()
        {
        }

        public void AddNewProduct(Product product)
        {
            if (_products.Any(x => x.Id == product.Id || x.Name == product.Name))
            {
                throw new Exception("Product already exists");
            }

            _products.Add(product);
        }

        public void RestockProduct(Guid productId, int amountToAdd)
        {
            if (amountToAdd < 1)
            {
                throw new Exception("Cannot restock less than one product");
            }
            var product = _products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                throw new Exception("Product does not exist");
            }
            product.Stock += amountToAdd;
        }
    }
}
