using Microsoft.AspNetCore.Mvc;
using RestWebAppl.Models;
using RestWebAppl.Models.ViewModels;
using RestWebAppl.Infrastructure;

namespace RestWebAppl.Controllers
{
    public class OrderController : Controller
    {
        public IRepository Repository { get; set; }
        private IOrderRepository orderRepository;
        private Cart cart;
        public OrderController(IOrderRepository OrdRep,IRepository itemrRep, Cart cartService)
        {
            orderRepository = OrdRep;
            Repository = itemrRep;
            cart = cartService;
        }

        public ViewResult Checkout()
        {

            return View(new Order());
        }
        [HttpGet]
        public ViewResult Item(Guid itemId) => View(Repository.Items.FirstOrDefault(a => a.Id == itemId));

        public IActionResult Cart() => View(new CartIndexViewModel { Cart = GetCart() });

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, Ваша корзина пустая");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                orderRepository.SaveOrder(order);
                return RedirectToAction("Home/Index");
            }
            else
            {
                return View(order);
            }
        }
        [HttpPost]
        public JsonResult AddToCart(Guid itemid, int quantity)
        {
            Item item = Repository.Items.FirstOrDefault(p => p.Id == itemid);
            if (item != null)
            {
                Cart cart = GetCart();
                cart.AddItem(item, quantity);
                SaveCart(cart);
            }
            Response response = new Response()
            {
                dateTime = DateTime.Now.ToShortTimeString(),
                status = true
            };
            return Json(response);
        }
        public RedirectToActionResult RemoveFormCart(Guid itemid)
        {
            Item item = Repository.Items
                .FirstOrDefault(p => p.Id == itemid);
            if (item != null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(item);
                SaveCart(cart);
            }
            if (cart.Lines.Count() == 1)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Cart");
        }
        [HttpPost]
        public RedirectToActionResult QuantityIncrement(Guid itemId)
        {
            cart.ChangeLine(itemId);
            SaveCart(cart);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public JsonResult QuantityDecrement(CartLine line)
        {
            if (line.Quantity > 1)
            {
                line.Quantity--;
            }
            return Json(line);
        }

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

    }
}
