using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Events
{
    public abstract class Event
    {

        public Guid Id { get; }

        public DateTime Date { get; }

        protected Event(Guid id, DateTime date)
        {
            Id = id;
            Date = date;
        }
    }
}
