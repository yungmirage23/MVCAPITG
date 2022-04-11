using Microsoft.AspNetCore.Mvc;
using mysecondshop.Models;
using System.Linq;

namespace mysecondshop.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository orderRepository;
        private Cart cart;
        public OrderController(IOrderRepository rep,Cart cartService)
        {
            orderRepository = rep;
            cart = cartService;
        }

        public ViewResult Checkout() => View(new Order());

        
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, Ваша корзина пустая");
            }
            if (ModelState.IsValid)
            {
                order.Lines= cart.Lines.ToArray();
                orderRepository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }
        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }



    }
}
