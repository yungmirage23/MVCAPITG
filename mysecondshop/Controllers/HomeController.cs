using Microsoft.AspNetCore.Mvc;
using RestWebAppl.Models;
using RestWebAppl.Models.ViewModels;

namespace RestWebAppl.Controllers
{
    public class HomeController : Controller
    {
        private IRepository Repository;
        public int PageSize = 15;

        public HomeController(IRepository repository)
        {
            Repository = repository;
        }
        public IActionResult Testing()
        {
            return View();
        }

        public IActionResult Index(int productPage = 1) => View(new ItemsListViewModel
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

        public IActionResult Delivery() => View("Delivery");

        public IActionResult Payment() => View("Payment");

        public IActionResult Support() => View("Support");

    }
}