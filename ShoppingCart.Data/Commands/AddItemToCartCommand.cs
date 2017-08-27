using System;
using System.Collections.Generic;
using System.Text;
using ShoppingCart.Data.Models;

namespace ShoppingCart.Data.Commands
{
    public class AddItemToCartCommand : Command
    {
        public Guid UserId { get; }

        public Product Product { get; }

        public AddItemToCartCommand(Guid userId, Product product)
        {
            UserId = userId;

            Product = product;
        }
    }
}
