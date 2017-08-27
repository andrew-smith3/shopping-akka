using System;
using ShoppingCart.Actors;
using ShoppingCart.Data.Models;

namespace ShoppingCart.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            Actors();
            System.Console.ReadKey();
        }

        static void Basic()
        {
            var cart = new Cart();
            cart.AddProductToCart(new Product { Name = "Foo" });
            cart.AddProductToCart(new Product { Name = "Bar" });
            cart.AddProductToCart(new Product { Name = "Foo" });

            System.Console.WriteLine(cart.ToString());
        }

        static void Actors()
        {
            var system = new CartSystem();

            var fred = Guid.NewGuid();
            var bob = Guid.NewGuid();

            var apple = new Product {Name = "Apple"};
            var cherry = new Product {Name = "Cherry"};

            system.AddItemToCart(fred, apple);
            System.Console.ReadKey();
            system.AddItemToCart(bob, cherry);
            system.AddItemToCart(bob, apple);
            system.AddItemToCart(fred, cherry);
            system.AddItemToCart(fred, cherry);
        }
    }
}
