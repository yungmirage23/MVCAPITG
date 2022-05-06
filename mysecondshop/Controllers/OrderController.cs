using Microsoft.AspNetCore.Mvc;
using RestWebAppl.Models;
using RestWebAppl.Models.ViewModels;
using RestWebAppl.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
namespace RestWebAppl.Controllers
{
    public class OrderController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IRepository Repository { get; set; }
        private IOrderRepository orderRepository;
        private Cart cart;
        private IReviewRepository reviewRepository;

        public OrderController(IOrderRepository OrdRep, IRepository itemrRep, Cart cartService, IReviewRepository reviewRep,UserManager<ApplicationUser> userMngr)
        {
            orderRepository = OrdRep;
            Repository = itemrRep;
            cart = cartService;
            reviewRepository = reviewRep;
            userManager = userMngr;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddReview(Guid itemId) 
        {
            var user = await userManager.GetUserAsync(User);
            var reviews = reviewRepository.Reviews.Where(i=>i.ItemId==itemId);
            var item = Repository.Items.FirstOrDefault(i => i.Id == itemId);
            var viewmodel = new ReviewAddViewModel() {Reviews=reviews,Item=item};
            return View(viewmodel); 
        }
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> AddReview(Guid itemId,string text)
        {
            var user = await userManager.GetUserAsync(User);
            var review = new Review() {Text = text,User=user,ItemId=itemId};
            reviewRepository.AddReview(review);
            return Json(itemId);
        }

        [HttpGet]
        public ViewResult Item(Guid itemId) => View(new ItemPageViewModel { item = Repository.Items.FirstOrDefault(a => a.Id == itemId), reviews = reviewRepository.Reviews.Where(r=>r.ItemId==itemId)});

        public IActionResult Cart() => View(new CartIndexViewModel { Cart = GetCart() });
        public IActionResult Checkout()
        {
            var Cart = GetCart();
            if (Cart.Lines.Count() == 0)
            {
                TempData["error"] = $"Извините, Ваша корзина пустая";
                return RedirectToAction("Shop", "Home");
            }
            return View(new Order());
        }
        [HttpPost]
        public JsonResult Checkout([FromBody] Order order)
        {
            var response = new Response();
            if (cart.Lines.Count() == 0)
            {
                TempData["error"] = $"Извините, Ваша корзина пустая";
                ModelState.AddModelError("", "Извините, Ваша корзина пустая");
            }
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    order.UserId = User.Identity.Name;
                }
                order.OrderStatus = "Принят";
                order.OrderTime = DateTime.Now.ToShortTimeString();
                order.OrderDate = DateTime.Now.ToShortDateString();
                order.Lines = cart.Lines.ToArray();
                orderRepository.SaveOrder(order);
                cart.Clear();
                response.dateTime = DateTime.Now.ToShortTimeString();
                response.status = true;
                response.returnUrl = "Home/Index";
                TempData["message"] = $"Заказ оформлен. Ожидайте, мы вам перезвоним";
                return Json(response);
            }
            else
            {
                response.status = false;
                response.dateTime = DateTime.Now.ToShortTimeString();
                response.returnUrl = "Order/Checkout";
                TempData["error"] = $"Произошла ошибка при оформлении заказа, обратитесь в службу поддержки";
                return Json(response);
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
                TempData["message"] = $"Товар успешно добавлен в корзину";
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
                cart.RemoveLine(itemid);
                SaveCart(cart);
                TempData["message"] = $"Товар успешно удалён из корзины";
            }            
            if (cart.Lines.Count() == 1)
            {
                TempData["error"] = $"Ваша корзина пустая";
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
            if (cart.Lines.Count() == 0)
            {
            TempData["message"] = $"Товар успешно удалён из корзины";
            }
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
            if (cart.Lines.Count() == 0)
            {
                TempData["message"] = $"Товар успешно удалён из корзины";
            }
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
