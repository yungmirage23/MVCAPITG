using Microsoft.AspNetCore.Mvc;
using mysecondshop.Models;
using mysecondshop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace mysecondshop.Controllers
{
    [Authorize]
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
          return View(repository.Items.FirstOrDefault(i=>i.Id==itemId));
        }
        [HttpPost]
        public IActionResult Edit(Item item, IFormFile MyImage)
        {
            if (ModelState.IsValid&&MyImage!=null)
            {
                var uniqueFileName = GetUniqueFileName(MyImage.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                MyImage.CopyTo(new FileStream(filePath, FileMode.Create));
                item.ImagePath= uniqueFileName;
                repository.SaveItem(item);
                TempData["message"] = $"Товар {item.Name} успешно сохранён";
                return RedirectToAction("Index");
            }
            else
            {
                return View(item);
            }
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
        [Authorize]
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
