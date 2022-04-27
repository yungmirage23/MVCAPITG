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

        public IActionResult Checkout()
        {
            var Cart = GetCart();
            if(Cart.Lines.Count() == 0)
            {
                return RedirectToAction("Shop","Home");
            }
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
        public IActionResult RemoveFormCart(Guid itemid,string ReturnUrl)
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
            return Redirect(ReturnUrl);
        }
        [HttpPost]
        public RedirectToActionResult QuantityIncrement(Guid itemId)
        {
            cart.AddQuantity(itemId);
            SaveCart(cart);
            return RedirectToAction("Cart");
        }
        [HttpPost]
        public RedirectToActionResult QuantityDecrement(Guid itemId)
        {
            cart.MinusQuantity(itemId);
            SaveCart(cart);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public JsonResult IncrementJson(Guid itemId)
        {
            cart.AddQuantity(itemId);
            SaveCart(cart);
            var result = new
            {
                Quantity = 0,
                Price = 0
            };
            if (cart.Lines.FirstOrDefault(p => p.Item.Id == itemId) != null)
            {
                var resultTrue = new
                {
                    Quantity = cart.Lines.FirstOrDefault(i => i.Item.Id == itemId).Quantity,
                    Price = cart.Lines.FirstOrDefault(i => i.Item.Id == itemId).Item.Price
                };
                return Json(resultTrue);
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult DecrementJson(Guid itemId)
        {
            cart.MinusQuantity(itemId);
            SaveCart(cart);
            var result = new
            {
                Quantity = 0,
                Price = 0
            };
            if (cart.Lines.FirstOrDefault(p => p.Item.Id == itemId)!=null)
            {
                var resultTrue = new
                {
                    Quantity = cart.Lines.FirstOrDefault(i => i.Item.Id == itemId).Quantity,
                    Price = cart.Lines.FirstOrDefault(i => i.Item.Id == itemId).Item.Price
                };
                return Json(resultTrue);
            }
            return Json(result);
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
