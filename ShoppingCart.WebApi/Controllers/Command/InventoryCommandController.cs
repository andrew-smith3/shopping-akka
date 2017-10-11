﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Actors;
using ShoppingCart.Data.Commands.Inventory;
using ShoppingCart.Data.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.WebApi.Controllers.Command
{
    [Route("api/inventory")]
    public class InventoryCommandController : Controller
    {
        private readonly CartSystem _cartSystem;

        public InventoryCommandController(CartSystem cartSystem)
        {
            _cartSystem = cartSystem;
        }

        [HttpPost]
        public async Task<IActionResult>  AddNewProduct([FromBody] NewProductDTO dto)
        {
            var command = new AddNewProductCommand(dto.Name, dto.Price, dto.Stock);
            var result =  await _cartSystem.AddNewProduct(command);
            if (result == "OK")
            {
                return Ok();
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("{productId}")]
        public async Task<IActionResult> RestockProduct(Guid productId, [FromBody] int amountToAdd)
        {
            var command = new RestockProductCommand(productId, amountToAdd);
            var result = await _cartSystem.RestockProduct(command);
            if (result == "OK")
            {
                return Ok();
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}