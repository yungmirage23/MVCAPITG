using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public interface IPromoCodeService
    {
        IQueryable<PromoCode> PromoCodes { get; }
        PromoCode CreatePromoCode(string Email);
        string CreateRandomCode();
        public PromoCode DeleteCode(PromoCode promoCode);
        public bool UsePromoCode(string Code);
    }
}
