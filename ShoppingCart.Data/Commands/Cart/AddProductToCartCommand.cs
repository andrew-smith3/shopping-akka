using System;

namespace ShoppingCart.Data.Commands.Cart
{
    public class AddProductToCartCommand : Command
    {
        public Guid UserId { get; }

        public Guid ProductId { get; }

        public AddProductToCartCommand(Guid userId, Guid productId)
        {
            UserId = userId;
            ProductId = productId;
        }
    }
}
