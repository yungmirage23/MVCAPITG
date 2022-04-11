using Microsoft.AspNetCore.Mvc;
using mysecondshop.Models;
namespace mysecondshop.Components
{
    public class CartSummaryViewComponent:ViewComponent
    {
        private Cart cart;
        public CartSummaryViewComponent(Cart cartservice)
        {
            cart = cartservice;
        }
        public IViewComponentResult Invoke()
        {
            return View(cart);
        }

    }
}
