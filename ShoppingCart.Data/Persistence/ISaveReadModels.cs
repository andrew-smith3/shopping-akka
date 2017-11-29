using System;
using System.Collections.Generic;
using System.Text;
using ShoppingCart.Data.ReadModels;

namespace ShoppingCart.Data.Persistence
{
    public interface ISaveReadModels 
    {
        void Save<T>(T readModel) where T : ReadModel;
    }
}
