using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Actors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.WebApi.Controllers.Command
{
    public abstract class CommandController : Controller
    {
        protected CartSystem CartSystem { get; }

        protected CommandController(CartSystem cartSystem)
        {
            CartSystem = cartSystem;
        }

    }
}
