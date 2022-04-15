namespace RestWebAppl.Models
{
    public interface IRepository
    {
        IQueryable<Item> Items { get; }
        void SaveItem(Item item);
        Item DeleteItem(Guid id);
    }
}
