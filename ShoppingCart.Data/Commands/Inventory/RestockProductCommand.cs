using System;

namespace ShoppingCart.Data.Commands.Inventory
{
    public class RestockProductCommand : Command
    {
        public Guid ProductId { get; }

        public int AmountToAdd { get; }

        public RestockProductCommand(Guid productId, int amountToAdd)
        {
            ProductId = productId;
            AmountToAdd = amountToAdd;
        }
    }
}
