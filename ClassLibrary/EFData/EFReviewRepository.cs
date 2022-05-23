using Microsoft.EntityFrameworkCore;
namespace RestWebAppl.Models
{
    public class EFReviewRepository : IReviewRepository
    {
        private ApplicationDbContext ctx;
        private AppIdentityDbContext identContext;
        public EFReviewRepository(AppIdentityDbContext ident, ApplicationDbContext contx)
        {
            ctx = contx;
            identContext = ident;
        }
        public IQueryable<Review> Reviews => identContext.Reviews.Include(o=>o.User);
        public void AddReview(Review review)
        {
            identContext.Reviews.Add(review);
            identContext.SaveChanges();
        }
    }
}
