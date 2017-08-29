using System;
using System.Collections.Generic;
using System.Text;
using ShoppingCart.Data.Models;

namespace ShoppingCart.Data.Events
{
    public class CartUpdated
    {
        public Cart Cart { get; }

        public CartUpdated(Cart cart)
        {
            Cart = cart;
        }
    }
}
