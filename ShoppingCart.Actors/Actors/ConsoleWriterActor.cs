using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;

namespace ShoppingCart.Actors.Actors
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
