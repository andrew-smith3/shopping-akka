using System;
using Akka.Actor;

namespace ShoppingCart.Actors
{
    public class ConsoleWriterActor : ReceiveActor
    {

        public static Props CreateProps()
        {
            return Props.Create(() => new ConsoleWriterActor());
        }

        public ConsoleWriterActor()
        {
            Receive<string>(x => Console.WriteLine(x));
        }
    }
}
