using System;

namespace ShoppingCart.Data.ReadModels
{
    public abstract class ReadModel
    {
        public Guid Id { get; }

        protected ReadModel(Guid id)
        {
            Id = id;
        }
    }
}
