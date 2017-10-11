using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ShoppingCart.Data.Projections
{
    public abstract class Projection
    {
        public abstract string Type { get; }

        public Guid Id { get; }
    
        public DateTime Created { get; }

        protected Projection() : this(Guid.NewGuid(), DateTime.Now)
        {
        }

        protected Projection(Guid id, DateTime created)
        {
            Id = id;
            Created = created;
        }
    }
}
