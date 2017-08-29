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

        public Guid UserId { get; }

        public DateTime Created { get; }

        protected Projection(Guid userId) : this(Guid.NewGuid(), userId, DateTime.Now)
        {
        }

        protected Projection(Guid id, Guid userId, DateTime created)
        {
            Id = id;
            UserId = userId;
            Created = created;
        }
    }
}
