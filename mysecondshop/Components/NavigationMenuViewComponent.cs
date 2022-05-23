using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using ClassLibrary.Models;
using Microsoft.IdentityModel.Tokens;

namespace RestWebAppl.Components
{
    //Returns list<string> of all categories for swiper with navigation
    public class NavigationMenuViewComponent:ViewComponent
    {
        readonly string apiRoute = "api/items/categories";
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<string> categories;
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            var a = await GetDataHttp<List<string>>.CreateAsync(apiRoute);
            categories = a.ResultData;
            return View(categories);
        }
    }
}
