using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Actors;
using ShoppingCart.Data.Commands.Cart;
using ShoppingCart.Data.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.WebApi.Controllers.Command
{
    [Route("api/cart")]
    public class CartCommandController : CommandController
    {
        public CartCommandController(CartSystem cartSystem) : base(cartSystem)
        {
        }

        [HttpPost]
        [Route("{userId}")]
        public async Task<IActionResult> Post(Guid userId, [FromBody]AddProductToCartDTO dto)
        {
            var command = new AddProductToCartCommand(userId, dto.ProductId);
            var result = await CartSystem.AddProductToCart(command);
            if (result.IsSuccessful)
            {
                return Ok();
            };
            return BadRequest(result.Message);
        }

        [HttpDelete]
        [Route("{userId}/products/{productId}")]
        public async Task<IActionResult> Post(Guid userId, Guid productId)
        {
            var command = new RemoveProductFromCartCommand(userId, productId);
            var result = await CartSystem.RemoveProductFromCart(command);
            if (result.IsSuccessful)
            {
                return Ok();
            };
            return BadRequest(result.Message);
        }
    }
}
