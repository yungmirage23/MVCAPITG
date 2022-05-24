using ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using RestWebAppl.Models;
using RestWebAppl.Models.ViewModels;

namespace RestWebAppl.Controllers
{
    public class HomeController : Controller
    {
        //Number of items on the start page
        public int PageSize = 15;

        //Shows start page and configuring Data list in case of category=null||category!=null
        public async Task<IActionResult> Index(string category, int productPage = 1)
        {
            IQueryable<Item> Ii=null;
            string apiurl = "api/items";
            var getItems = await GetDataHttp<List<Item>>.CreateAsync(apiurl);
            
            if (getItems.ResponseMessage.IsSuccessStatusCode)
                Ii = getItems.ResultData.AsQueryable();
            else
                return BadRequest();
            
            if (category == null)
            {
                return View(new ItemsListViewModel()
                {
                    Items = Ii.Skip((productPage - 1) * PageSize).Take(PageSize),
                    PagingInfo = new PagingInfo()
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = Ii.Count()
                    }
                });
            }
            return View(new ItemsListViewModel
            {
                Items = Ii.Where(p => category == null || p.Category == category)
                        .Skip((productPage - 1) * PageSize)
                        .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null
                            ? Ii.Count()
                            : Ii.Where(e => e.Category == category).Count()
                },
                CurrentCategory = category
            });
        }
        //Delivery page
        public IActionResult Delivery() => View("Delivery");

        public IActionResult Payment() => View("Payment");

        public IActionResult Support() => View("Support");
    }
}