using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingCart.Data.Projections;
using ShoppingCart.Data.ProjectionStore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.WebApi.Controllers.Query
{
    [Route("api/inventory")]
    public class InventoryQueryController : Controller
    {
        private SqliteProjectionStore _projectionStore;

        public InventoryQueryController(SqliteProjectionStore projectionStore)
        {
            _projectionStore = projectionStore;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(JsonConvert.DeserializeObject<InventoryProjection>(_projectionStore.Retrieve(Guid.Empty, InventoryProjection.ProjectionType)));
        }
    }
}

