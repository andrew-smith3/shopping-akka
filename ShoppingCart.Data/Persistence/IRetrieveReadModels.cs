using System;
using ShoppingCart.Data.ReadModels;

namespace ShoppingCart.Data.Persistence
{
    public interface IRetrieveReadModels
    {
        T Retrieve<T>(Guid id) where T : ReadModel;
    }
}
