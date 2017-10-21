using System;

namespace ShoppingCart.Data.Queries.Inventory
{
    public class ProductQuery : Query
    {
        public Guid ProductId { get; }

        public ProductQuery(Guid productId)
        {
            ProductId = productId;
        }
    }
}
