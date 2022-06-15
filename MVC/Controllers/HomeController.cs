using ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using RestaurantMVC.Infrastructure.EmailService;
using RestWebAppl.Models;
using RestWebAppl.Models.ViewModels;

namespace RestWebAppl.Controllers
{
    public class HomeController : Controller
    {
        //Number of items on the start page
        public int PageSize = 15;
        IEmailSender _emailSender;
        IPromoCodeService _promoCodeService;
        public HomeController(IEmailSender emailSender, IPromoCodeService promoCodeService)
        {
            _emailSender=emailSender;
            _promoCodeService=promoCodeService;
        }

        //Shows start page and configuring Data list in case of category=null||category!=null
        [HttpGet]
        public async Task<IActionResult> Index(string category, int productPage = 1)
        {
            //if(category!=null)
            //category = category.Replace(" ","");
            IQueryable<Item> Ii;
            int itemsCount=PageSize;
            if(category == null)
            {
                string apiurl = $"api/items/page/{productPage}";
                var getItems = await GetDataHttp<List<Item>>.CreateAsync(apiurl);

                if (getItems.ResponseMessage.IsSuccessStatusCode)
                    Ii = getItems.ResultData.AsQueryable();
                else return BadRequest();
                string apiCount = $"api/items/count";
                var getCount = await GetDataHttp<int>.CreateAsync(apiCount);
                if (getCount.ResponseMessage.IsSuccessStatusCode)
                {
                    itemsCount = getCount.ResultData;
                }
                else return BadRequest();

                return View(new ItemsListViewModel()
                {
                    Items = Ii,
                    PagingInfo = new PagingInfo()
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = itemsCount
                    }
                });
            }
            else
            {
                string apiurl = $"api/items/category/{category}/page/{productPage}";
                var getItems = await GetDataHttp<List<Item>>.CreateAsync(apiurl);
                if (getItems.ResponseMessage.IsSuccessStatusCode)
                    Ii = getItems.ResultData.AsQueryable();
                else return BadRequest();

                string apiCount = $"api/items/count/{category}";
                var getCount = await GetDataHttp<int>.CreateAsync(apiCount);
                if (getCount.ResponseMessage.IsSuccessStatusCode)
                {
                    itemsCount = getCount.ResultData;
                }
                else return BadRequest();

                return View(new ItemsListViewModel()
                {
                    Items = Ii,
                    PagingInfo = new PagingInfo()
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = itemsCount
                    },
                    CurrentCategory= category
                });
            }
        }
        [HttpGet]
        public PartialViewResult SubscribeEmail() => PartialView();

        [HttpPost]
        public JsonResult SubscribeEmail(string Email)
        {
            var code = _promoCodeService.CreatePromoCode(Email);
            var message = new Message(new string[] { Email }, "Подписка на рассылку предложений", "Добрый день! Спасибо, за то что подписались на нашу рассылку,\n теперь вы будете получать выгодные предложения\n"+
                $"на свой почтовый аккаунт - {code.Email}.\n" +
                $"Ваш промо код :{code.Code}");
            _emailSender.SendEmail(message);
            return Json(new { success = true });
        }

        //Delivery page
        public IActionResult Delivery() => View("Delivery");

        public IActionResult Payment() => View("Payment");

        public IActionResult Support() => View("Support");
    }
}