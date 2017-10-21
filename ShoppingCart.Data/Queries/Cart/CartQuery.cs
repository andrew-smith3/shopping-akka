using System;

namespace ShoppingCart.Data.Queries.Cart
{
    public class CartQuery : Query
    {
        public Guid UserId { get; }

        public CartQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
