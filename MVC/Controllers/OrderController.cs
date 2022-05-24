using ClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestWebAppl.Infrastructure;
using RestWebAppl.Models;
using RestWebAppl.Models.ViewModels;

namespace RestWebAppl.Controllers
{
    public class OrderController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IOrderRepository orderRepository;
        private Cart cart;
        private IReviewRepository reviewRepository;

        public OrderController(IOrderRepository OrdRep, Cart cartService, IReviewRepository reviewRep, UserManager<ApplicationUser> userMngr)
        {
            orderRepository = OrdRep;
            cart = cartService;
            reviewRepository = reviewRep;
            userManager = userMngr;
        }
        //Shows View for creating review
        //Let user to write review only if he is authorized
        //Gets item from api 
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddReview(Guid itemId)
        {
            var user = await userManager.GetUserAsync(User);
            var reviews = reviewRepository.Reviews.Where(i => i.ItemId == itemId);

            string apiurl = $"api/items/{itemId}";
            var getItem = await GetDataHttp<Item>.CreateAsync(apiurl);
            if (getItem.ResponseMessage.IsSuccessStatusCode)
            {
                var viewmodel = new ReviewAddViewModel() { Reviews = reviews, Item = getItem.ResultData };
                return View(viewmodel);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> AddReview(Guid itemId, string text)
        {
            var user = await userManager.GetUserAsync(User);
            var review = new Review() { Text = text, User = user, ItemId = itemId };
            reviewRepository.AddReview(review);
            return Json(itemId);
        }
        //Gets item from api and returns it to view
        [HttpGet]
        public async Task<IActionResult> Item(Guid itemId)
        {
            string apiurl = $"api/items/{itemId}";
            var getItem = await GetDataHttp<Item>.CreateAsync(apiurl);
            if (getItem.ResponseMessage.IsSuccessStatusCode)
            {
                var item = getItem.ResultData;
                return View(new ItemPageViewModel { item = item, reviews = reviewRepository.Reviews.Where(r => r.ItemId == itemId) });
            }
            return StatusCode(404);
        }

        // Gets 
        public IActionResult Cart() => View(new CartIndexViewModel { Cart = GetCart() });

        public IActionResult Checkout()
        {
            var Cart = GetCart();
            if (Cart.Lines.Count() == 0)
            {
                TempData["error"] = $"Извините, Ваша корзина пустая";
                return RedirectToAction("Index", "Home");
            }
            return View(new Order());
        }

        [HttpPost]
        public JsonResult Checkout([FromBody] Order order)
        {
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
                TempData["message"] = $"Заказ оформлен. Ожидайте, мы вам перезвоним";
                return Json("response");
            }
            TempData["error"] = $"Произошла ошибка при оформлении заказа, обратитесь в службу поддержки";
            return Json("response");
        }

        [HttpPost]
        public async Task<JsonResult> AddToCart(Guid itemId, int quantity)
        {
            string apiurl = $"api/items/{itemId}";
            var getItem = await GetDataHttp<Item>.CreateAsync(apiurl);
            if (getItem.ResponseMessage.IsSuccessStatusCode && getItem.ResultData != null)
            {
                var item = getItem.ResultData;
                Cart cart = GetCart();
                cart.AddItem(item, quantity);
                SaveCart(cart);
                TempData["message"] = $"Товар успешно добавлен в корзину";
                return Json("response");
            }
            return Json("bad request");
        }

        public async Task<IActionResult> RemoveFormCart(Guid itemId, string ReturnUrl)
        {
            string apiurl = $"api/items/{itemId}";
            var getItem = await GetDataHttp<Item>.CreateAsync(apiurl);
            if (getItem.ResponseMessage.IsSuccessStatusCode && getItem.ResultData != null)
            {
                var item = getItem.ResultData;
                Cart cart = GetCart();
                cart.RemoveLine(itemId);
                SaveCart(cart);
                TempData["message"] = $"Товар успешно удалён из корзины";
                return Json("response");
            }
            if (cart.Lines.Count() == 1)
            {
                TempData["error"] = $"Ваша корзина пустая";
                return RedirectToAction("Index", "Home");
            }
            return Redirect(ReturnUrl);
        }
        // First Implementation for Increment with ajax html() replacement
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

        //Second Implementation via json  
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
        // Using sessions for using Cart via cookies// Finds string in sessions with using key
        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
        // Saves Cart // Adding string to Sessions, using key 
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
    }
}