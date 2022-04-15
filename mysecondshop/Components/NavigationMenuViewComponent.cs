using Microsoft.AspNetCore.Mvc;
using RestWebAppl.Models;
using System.Linq;
namespace RestWebAppl.Components
{
    public class NavigationMenuViewComponent:ViewComponent
    {
        private IRepository repository;
        public NavigationMenuViewComponent(IRepository rep)
        {
            repository = rep;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Items
            .Select(x => x.Category)
            .Distinct());
        }
    }
}
