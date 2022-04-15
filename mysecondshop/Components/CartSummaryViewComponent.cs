﻿using Microsoft.AspNetCore.Mvc;
using RestWebAppl.Models;
namespace RestWebAppl.Components
{
    public class CartSummaryViewComponent:ViewComponent
    {
        private Cart cart;
        public CartSummaryViewComponent(Cart cartservice)
        {
            cart = cartservice;
        }
        public IViewComponentResult Invoke()
        {
            return View(cart);
        }

    }
}
