using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace RestWebAppl.Models
{
    public class EFOrderRepository:IOrderRepository
    {
        private ApplicationDbContext context;
        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Order> Orders => context.Orders.Include(o => o.Lines).ThenInclude(l => l.Item);
        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Item));
            if(order.OrderID==0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}
