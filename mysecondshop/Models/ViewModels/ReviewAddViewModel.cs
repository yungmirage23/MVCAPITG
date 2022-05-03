namespace RestWebAppl.Models.ViewModels
{
    public class ReviewAddViewModel
    {
        public IQueryable<Review> Reviews { get; set;}
        public Item Item { get; set; }
    }
}
