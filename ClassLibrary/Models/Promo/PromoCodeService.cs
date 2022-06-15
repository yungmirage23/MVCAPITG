using ClassLibrary.Models;
using RestWebAppl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class PromoCodeService: IPromoCodeService
    {
        private ApplicationDbContext context;
        public PromoCodeService(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<PromoCode> PromoCodes => context.PromoCodes;
        public PromoCode CreatePromoCode(string Email)
        {
            var promoCode=new PromoCode();
            promoCode.Email = Email;
            promoCode.Code = CreateRandomCode();
            while (context.PromoCodes.FirstOrDefault(c => c.Code == promoCode.Code)!=null)
            {
                promoCode.Code = CreateRandomCode();
            }            
            context.PromoCodes.Add(promoCode);
            context.SaveChanges();
            return promoCode;
        }
        
        public bool UsePromoCode(string Code)
        {
            var code = context.PromoCodes.FirstOrDefault(c=>c.Code==Code&&c.IsActive);
            if (code != null)
            {
                code.IsActive = true;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public PromoCode DeleteCode(PromoCode promoCode)
        {
            context.Remove(promoCode);
            context.SaveChanges();
            return promoCode;
        }

        public string CreateRandomCode() 
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[5];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars);
        }

    }
}
