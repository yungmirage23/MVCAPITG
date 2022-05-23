﻿namespace RestWebAppl.Models
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get;}
        void SaveOrder(Order order);
        
    }
}
