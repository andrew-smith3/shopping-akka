﻿using System;
using System.Collections.Generic;

namespace ShoppingCart.Data.ReadModels
{
    public class InventoryReadModel : ReadModel
    {
        public List<ProductReadModel> Products { get; }

        public InventoryReadModel(Guid id) : base(id)
        {
            Products = new List<ProductReadModel>();
        }

        public InventoryReadModel(Guid id, List<ProductReadModel> products) : base(id)
        {
            Products = products;
        }
    }
}
