using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Commands.Cart
{
    public class RemoveProductFromCartCommand : Command
    {
        public Guid UserId { get; }

        public Guid ProductId { get; }

        public RemoveProductFromCartCommand(Guid userId, Guid productId)
        {
            UserId = userId;
            ProductId = productId;
        }
    }
}
