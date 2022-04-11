namespace mysecondshop.Models.ViewModels
{
    public class ItemsListViewModel
    {
        public IQueryable<Item> Items { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }

    }
}
