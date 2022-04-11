namespace mysecondshop.Models
{
    public class EFRepository:IRepository
    {
    private ApplicationDbContext context;
    public EFRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
    public IQueryable<Item> Items=>context.Items;
    public void SaveItem(Item item)
    {
        if(item.Id == default(Guid))
            context.Items.Add(item);
        else
            {
            Item dbEntry=context.Items.FirstOrDefault(p=>p.Id==item.Id);
            if(dbEntry != null)
            {
                dbEntry.Name = item.Name;
                dbEntry.Description = item.Description;
                dbEntry.Category = item.Category;
                dbEntry.Price = item.Price;
                dbEntry.addedTime = item.addedTime;
                dbEntry.ImagePath = item.ImagePath;
            }
            }
        context.SaveChanges();
    }
    public Item DeleteItem(Guid id)
    {
        Item dbEntry=context.Items.FirstOrDefault(p=>p.Id == id);
            if (dbEntry != null)
            {
                context.Items.Remove(dbEntry);
                context.SaveChanges();
            }
        return dbEntry;
    }
    }
    
}
