﻿using System;
using ShoppingCart.Actors;
using ShoppingCart.Domain;

namespace ShoppingCart.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            Actors();
            System.Console.WriteLine("Done");
            System.Console.ReadKey();
        }

        public static void Basic()
        {
//            var cart = new Cart(Guid.NewGuid());
//            cart.AddProductToCart(new Product { Name = "Foo" });
//            cart.AddProductToCart(new Product { Name = "Bar" });
//            cart.AddProductToCart(new Product { Name = "Foo" });
//
//            System.Console.WriteLine(cart.ToString());
        }

        public static void Actors()
        {
            var system = new CartSystem();

            var fred = Guid.Parse("b3e1e5c3-83e7-466b-9165-639e95db1342");
            var bob = Guid.Parse("484ab3db-0fd2-4342-88cf-f1e1f13427f0");

            var apple = new Product {Name = "Apple"};
            var cherry = new Product {Name = "Cherry"};

//            system.AddItemToCart(fred, apple);
//            system.AddItemToCart(bob, cherry);
//            system.AddItemToCart(bob, apple);
//            system.AddItemToCart(fred, cherry);
//            system.AddItemToCart(fred, cherry);
        }
    }
}
