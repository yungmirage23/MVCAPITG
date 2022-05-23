# Restourant delivery web application

### Реализация веб приложения на Asp Net Core MVC с использованием WebApi


## 1. WebApi
### Основные API
#### 1) Получить список всех товаров, размещённых в репозитории
```csharp
//GET api/items
[HttpGet]
public IEnumerable<Item> Get() => itemRepository.Items.ToList();
```
![Alt Text](https://s8.gifyu.com/images/api_items.gif)
#### 2) Получить товар по уникальному идентификатору
```csharp
//GET api/items/F74349D5-52B4-4A4A-0382-08DA02C684C5
[HttpGet("{id:guid}")]
 public ActionResult<Item> Get(Guid id)
{
  var item = itemRepository.Items.First(g => g.Id == id);
  if (item != null)
    return item;
  return BadRequest();
}
![Alt Text](https://s8.gifyu.com/images/api_items_id.gif)
```
#### 3) Получить по 15 товаров в зависимости от указаной страницы
```csharp
//GET api/items/take/2
[HttpGet("take/{productPage:int}")]
public IEnumerable<Item> Get(int productPage)=>itemRepository.Items
  .Skip((productPage - 1) * PageSize)
  .Take(PageSize);
```
#### 4) Добавить новый или изменить существующий товар
```csharp
//POST api/items/add/{item}
[HttpPost("add")]
public async Task<ActionResult<Item>> Post(Item item)
{
  try
  {
    if (item == null)
    {
      return BadRequest();
    }
    var repitem = itemRepository.Items.FirstOrDefault(i => i.Id == item.Id);
    if (repitem != null)
    {
      repitem.Description = item.Description;
      repitem.Category = item.Category;
      repitem.Price = item.Price;
      repitem.Name = item.Name;
      repitem.addedTime = item.addedTime;
      itemRepository.SaveItem(repitem);
    }
    itemRepository.SaveItem(item);
    return Ok();
  }
  catch (Exception ex)
  {
    return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
  }
}
```
## 2. MVC Application
