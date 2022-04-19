using Microsoft.AspNetCore.Mvc;
using RestWebAppl.Infrastructure;
using RestWebAppl.Models;
using RestWebAppl.Models.ViewModels;

namespace RestWebAppl.Controllers
{
    public class HomeController : Controller
    {
        private IRepository Repository;
        private Cart cart;
        public int PageSize = 15;

        public HomeController(IRepository repository, Cart cartService)
        {
            Repository = repository;
            cart = cartService;
        }
        public IActionResult Testing()
        {
            return View();
        }



        public IActionResult Index(int productPage=1)=>View(new ItemsListViewModel
        {
            Items = Repository.Items
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
            PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = Repository.Items.Count()
            },
        });
        public IActionResult Shop(string category, int productPage = 1) => View(new ItemsListViewModel
        {
            Items = Repository.Items
                .Where(p => category == null || p.Category == category)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
            PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = category == null ? Repository.Items.Count() :
                    Repository.Items.Where(e => e.Category == category).Count()
            },
            CurrentCategory = category
        });
        [HttpGet]
        public ViewResult Order(Guid itemId) => View(Repository.Items.FirstOrDefault(a => a.Id == itemId));
        public IActionResult Delivery() => View("Delivery");

        public IActionResult Payment() => View("Payment");

        public IActionResult Support() => View("Support");
        public IActionResult Cart() => View(new CartIndexViewModel { Cart= GetCart()});
        [HttpPost]
        public JsonResult AddToCart(Guid itemid)
        {
            Item item =Repository.Items.FirstOrDefault(p=>p.Id == itemid);
            if (item != null)
            {
                Cart cart = GetCart();
                cart.AddItem(item, 1);
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
                return RedirectToAction("Index");
            }
            return RedirectToAction("Cart");
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
    
    /*
       public ViewResult Cart(string returnUrl)
       {
           return View(new CartIndexViewModel { Cart = GetCart(), ReturnUrl = returnUrl });
       }

      public RedirectToActionResult AddToCart(Guid id, string returnUrl)
       {
           Item item = Repository.Items
           .FirstOrDefault(p => p.Id == id);
           if (item != null)
           {
               Cart cart = GetCart();
               cart.AddItem(item, 1);
               SaveCart(cart);
           }
           return RedirectToAction("Shop", new { returnUrl });
       }

       public RedirectToActionResult RemoveFormCart(Guid id, string returnUrl)
       {
           Item item = Repository.Items
               .FirstOrDefault(p => p.Id == id);
           if (item != null)
           {
               Cart cart = GetCart();
               cart.RemoveLine(item);
               SaveCart(cart);
           }
           return RedirectToAction("Shop", new { returnUrl });
       }*/
}