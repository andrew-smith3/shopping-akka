using System;

namespace ShoppingCart.Data.Queries.Inventory
{
    public class ProductStockStatusQuery : Query
    {
        public Guid ProductId { get; }

        public ProductStockStatusQuery(Guid productId)
        {
            ProductId = productId;
        }
    }
}
