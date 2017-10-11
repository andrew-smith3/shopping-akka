using System;

namespace ShoppingCart.Data.Events
{
    public class ProductRestocked
    {
        public Guid ProductId { get; }

        public int AmountAdded { get; }

        public ProductRestocked(Guid productId, int amountAdded)
        {
            ProductId = productId;
            AmountAdded = amountAdded;
        }
    }
}
