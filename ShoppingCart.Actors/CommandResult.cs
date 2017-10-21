using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Actors
{
    public class CommandResult
    {
        public static CommandResult Success()
        {
            return new CommandResult(true, string.Empty);
        }

        public static CommandResult Error(string message)
        {
            return new CommandResult(false, message);
        }

        public bool IsSuccessful { get; }

        public string Message { get; }

        private CommandResult(bool success, string message)
        {
            IsSuccessful = success;
            Message = message;
        }
    }
}
