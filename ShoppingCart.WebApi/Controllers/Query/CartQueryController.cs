using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Actors;
using ShoppingCart.Data.Queries.Cart;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.WebApi.Controllers.Query
{
    [Route("api/cart")]
    public class CartQueryController : Controller
    {
        private readonly CartSystem _cartSystem;

        public CartQueryController(CartSystem cartSystem)
        {
            _cartSystem = cartSystem;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var cart = await _cartSystem.GetCart(new CartQuery(userId));
            return Ok(cart);
        }
    }
}
