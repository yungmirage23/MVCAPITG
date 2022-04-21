using System.Linq;
using System.Collections.Generic;
namespace RestWebAppl.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection=new List<CartLine>();
        public virtual void AddItem(Item item, int quantity)
        {
            CartLine line = lineCollection
                .Where(p=>p.Item.Id==item.Id)
                .FirstOrDefault();
            if (line == null)
                lineCollection.Add(new CartLine { Item = item, Quantity = quantity });
            else
                line.Quantity += quantity;
        }
        public virtual void RemoveLine(Item item)
        {
            lineCollection.RemoveAll(l=>l.Item.Id==item.Id);
        }
        public virtual void ChangeLine(Guid itemId)
        {
            if (itemId!=null)
            {
                lineCollection.FirstOrDefault(p=>p.Item.Id==itemId).Quantity++;
                var a = lineCollection.FirstOrDefault(p => p.Item.Id == itemId);
            } 
        }
        public virtual void Clear()=>lineCollection.Clear();
        public virtual decimal ComputeTotalValue()=>lineCollection.Sum(s=>s.Item.Price * s.Quantity);
        public IEnumerable<CartLine> Lines => lineCollection;
        


    }
        


    public class CartLine
    {
        public int CartLineId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
