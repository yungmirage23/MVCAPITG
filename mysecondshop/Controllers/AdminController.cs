using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestWebAppl.Models;
using RestWebAppl.Models.ViewModels;
using System.Net.Http.Headers;
using ClassLibrary.Models;

namespace RestWebAppl.Controllers
{
    //Available for admin role 
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // For image download 
        private readonly IWebHostEnvironment env;
        private IOrderRepository orderRepository;

        public AdminController(IOrderRepository orep, IWebHostEnvironment environment)
        {
            orderRepository = orep;
            env = environment;
        }
        //Gets all items from api 
        public async Task<IActionResult> Index()
        {
            IEnumerable<Item> Ii;
            string apiurl = "api/items";
            var getItems = await GetDataHttp<List<Item>>.CreateAsync(apiurl);

            if (getItems.ResponseMessage.IsSuccessStatusCode)
                Ii = getItems.ResultData.AsQueryable();
            else
                return BadRequest();

            return View(Ii);
        }
        //takes item from api and returns it to view if item==null redirect to create action
        public async Task<IActionResult> Edit(Guid itemId)
        {
            Item item = new Item();
            if (itemId == null)
            {
                return RedirectToAction("Create");
            }
            string apiurl = $"api/items/{itemId}";
            var getItem = await GetDataHttp<Item>.CreateAsync(apiurl);

            if (getItem.ResponseMessage.IsSuccessStatusCode)
                item = getItem.ResultData;
            else
                return BadRequest();
            return View(item);
        }
        //Changes item from api result 
        [HttpPost]
        public async Task<IActionResult> Edit(Item item/*, IFormFile? MyImage*/)
        {
            if (ModelState.IsValid)
            {
                string apiurl = "api/items/add";
                var postItem = await PostDataHttp<Item>.CreateAsync(apiurl, item);
                if (postItem.ResponseMessage.IsSuccessStatusCode)
                    {
                        TempData["message"] = $"Товар {item.Name} успешно сохранён";
                        return RedirectToAction("Index");
                    }
            }
            TempData["error"] = $"Товар {item.Name} НЕ СОХРАНЁН";
            return View(item);
            /*if (MyImage != null)
            {
                var uniqueFileName = GetUniqueFileName(MyImage.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                MyImage.CopyTo(new FileStream(filePath, FileMode.Create));
                repitem.ImagePath = uniqueFileName;
                repository.SaveItem(repitem);
            }*/
            
        }
        public ViewResult Create() => View("Edit", new Item() { addedTime = DateTime.Now.ToLongTimeString() });

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            string apiurl = $"api/items/{id}";
            var getItem = await GetDataHttp<Item>.CreateAsync(apiurl);
            if (getItem.ResponseMessage.IsSuccessStatusCode)
            {
                var item = getItem.ResultData;
                apiurl = $"api/items/{id}";
                var postItem = await DeleteDataHttp<Guid>.CreateAsync(apiurl, id);
                if (postItem.ResponseMessage.IsSuccessStatusCode)
                {
                    TempData["message"] = $"{item.Name} был удалён";
                }
                else
                    TempData["error"] = "Ошибка при удалении продукта";
            }
            else
            {
                TempData["error"] = "Ошибка, продукта не существует";
            }
            return RedirectToAction("Index");
        }

        public ViewResult List() => View(orderRepository.Orders.Where(o => !o.Shipped));

        //Marks item as shipped
        [HttpPost]
        public IActionResult MarkShipped(int orderId)
        {
            Order? order = orderRepository.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order != null)
            {
                order.Shipped = true;
                orderRepository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public IActionResult AddImage() => View(new ImageViewModel());

        [HttpPost]
        public IActionResult AddImage(ImageViewModel model)
        {
            if (model.MyImage != null)
            {
                var uniqueFileName = GetUniqueFileName(model.MyImage.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.MyImage.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            return RedirectToAction("Index");
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString().Substring(0, 4) + Path.GetExtension(fileName);
        }
    }
}