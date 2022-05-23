namespace RestWebAppl.Models
{
    public class Review
    {
        public int Id { get; set; }
        public Guid ItemId { get; set; }
        public string Text { get; set; }        
        public ApplicationUser User { get; set; }
    }
}
