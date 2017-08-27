using System;
using System.Collections.Generic;
using System.Text;
using ShoppingCart.Data.Commands;

namespace ShoppingCart.Data
{
    public class CommandHandler
    {

        public CommandHandler()
        {
            
        }

        public void Handle(Command command)
        {
            switch (command)
            {
                case AddItemToCartCommand c:
                    AddItemToCart(c);
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        private static void AddItemToCart(AddItemToCartCommand command)
        {
            
        }
    }
}
