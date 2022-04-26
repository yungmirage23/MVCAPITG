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
        public virtual void AddQuantity(Guid itemId)
        {
            if (itemId!=null)
            {
                lineCollection.FirstOrDefault(p=>p.Item.Id==itemId).Quantity++;
            } 
        }
        public virtual void MinusQuantity(Guid itemId)
        {
            if (itemId != null)
            {
                if (lineCollection.FirstOrDefault(p => p.Item.Id == itemId).Quantity > 1)
                {
                    lineCollection.FirstOrDefault(p => p.Item.Id == itemId).Quantity--;
                }
                else
                lineCollection.RemoveAll(l => l.Item.Id==itemId);
            }
        }
        public virtual void Clear()=>lineCollection.Clear();
        public IEnumerable<CartLine> Lines => lineCollection;
        
        public virtual decimal ComputeTotalValue()
        {
            decimal Total=0;
            foreach(var line in lineCollection)
            {
                Total+=line.Item.Price * line.Quantity;
            }
            return Total;
        }

    }
        


    public class CartLine
    {
        public int CartLineId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public virtual decimal ComputeLineTotal()=>(Item.Price * Quantity);
    }
}
