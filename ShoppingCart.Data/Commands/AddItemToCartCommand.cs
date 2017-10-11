using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Commands
{
    public class AddItemToCartCommand : Command
    {
        public Guid UserId { get; }

        public Guid ProductId { get; }

        public AddItemToCartCommand(Guid userId, Guid productId)
        {
            UserId = userId;
            ProductId = productId;
        }
    }
}
