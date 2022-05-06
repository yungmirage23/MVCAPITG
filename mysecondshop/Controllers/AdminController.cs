using Microsoft.AspNetCore.Mvc;
using RestWebAppl.Models;
using RestWebAppl.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace RestWebAppl.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment env;
        private IRepository repository;
        private IOrderRepository orderRepository;

        public AdminController(IRepository rep, IOrderRepository orep, IWebHostEnvironment environment)
        {
            repository = rep;
            orderRepository = orep;
            env = environment;
        }

        public IActionResult Index()=>View(repository.Items);

        public ViewResult Edit(Guid itemId)
        {
            var item = repository.Items.FirstOrDefault(i => i.Id == itemId);
            if(item!= null)
            {
                return View(item);
            }
            else
                return View(item = new Item());
            
                     
        }
        [HttpPost]
        public IActionResult Edit(Item item, IFormFile? MyImage)
        {
            var repitem=repository.Items.FirstOrDefault(i=>i.Id==item.Id);
            if(repitem!= null)
            {
                if (ModelState.IsValid)
                {
                    TempData["message"] = $"Товар {item.Name} успешно сохранён";
                    repitem.Description = item.Description;
                    repitem.Category = item.Category;
                    repitem.Price = item.Price;
                    repitem.Name= item.Name;
                    repitem.addedTime = item.addedTime;
                    repository.SaveItem(repitem);
                }
                else
                {
                    TempData["error"] = $"Товар {item.Name} НЕ СОХРАНЁН";
                    return View(item);
                }
                if (MyImage != null)
                {
                    var uniqueFileName = GetUniqueFileName(MyImage.FileName);
                    var uploads = Path.Combine(env.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    MyImage.CopyTo(new FileStream(filePath, FileMode.Create));
                    repitem.ImagePath = uniqueFileName;
                    repository.SaveItem(repitem);
                }
            }
            return RedirectToAction("Index");
        }
        public ViewResult Create(ItemsListViewModel pvm) => View("Edit", new Item() { addedTime=DateTime.Now.ToLongTimeString()});
        [HttpPost]
        public IActionResult Delete(Guid id) 
        {
            Item DeletedItem=repository.DeleteItem(id);
            if (DeletedItem != null)
            {
                TempData["message"] = $"{DeletedItem.Name} был удалён";
            }
           return RedirectToAction("Index");
        }
        public ViewResult List() => View(orderRepository.Orders.Where(o => !o.Shipped));
        [HttpPost]
        public IActionResult MarkShipped(int orderId)
        {
            Order order = orderRepository.Orders.FirstOrDefault(o => o.OrderID == orderId);
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
            if(model.MyImage != null)
            {
                var uniqueFileName = GetUniqueFileName(model.MyImage.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath=Path.Combine(uploads, uniqueFileName);
                model.MyImage.CopyTo(new FileStream(filePath,FileMode.Create));
            }
            return RedirectToAction("Index");
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName=Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)+"_"+Guid.NewGuid().ToString().Substring(0,4)+Path.GetExtension(fileName);
        }

    }
}
