using Microsoft.AspNetCore.Mvc;
using mysecondshop.Models;
using mysecondshop.Models.ViewModels;
namespace mysecondshop.Components
{
    public class CartViewComponent:ViewComponent
    {
        private CartIndexViewModel _cartIndexViewModel=new CartIndexViewModel();
        public CartViewComponent(Cart cartservice)
        {
            _cartIndexViewModel.ReturnUrl = "";
            _cartIndexViewModel.Cart = cartservice;
        }
        public IViewComponentResult Invoke()
        {
            return View(_cartIndexViewModel);
        }
    }
}
