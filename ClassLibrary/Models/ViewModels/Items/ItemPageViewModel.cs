namespace RestWebAppl.Models.ViewModels
{
    public class ItemPageViewModel
    {
        public Item item { get; set; }
        public IQueryable<Review>? reviews { get; set; }
    }
}
