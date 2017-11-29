using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingCart.Actors;
using ShoppingCart.Data.Persistence;
using ShoppingCart.Data.Projections;
using ShoppingCart.Data.Queries.Inventory;
using ShoppingCart.Data.ReadModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.WebApi.Controllers.Query
{
    [Route("api/inventory")]
    public class InventoryQueryController : Controller
    {
        private readonly CartSystem _cartSystem;

        public InventoryQueryController(CartSystem cartSystem)
        {
            _cartSystem = cartSystem;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var inventory = await _cartSystem.GetInventory(new InventoryQuery());
            return Ok(inventory);
        }
    }
}

