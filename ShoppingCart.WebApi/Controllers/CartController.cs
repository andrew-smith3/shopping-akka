using System;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data.ProjectionStore;
using ShoppingCart.Data.Projections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CartController : Controller
    {

        [HttpGet("{userId}")]
        public string Get(Guid userId)
        {
            var store = new SqliteProjectionStore();
            return store.Retrieve(userId, CartProjection.ProjectionType);
        }
    }
}
