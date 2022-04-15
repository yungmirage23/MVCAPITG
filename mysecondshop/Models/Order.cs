using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace RestWebAppl.Models
{
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }
        
        [BindNever]
        public ICollection<CartLine>? Lines { get; set; }
        [BindNever]
        public bool Shipped { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите Ваше имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите Ваш адрес")]
        public string Line1 { get; set; }
        [Required(ErrorMessage ="Пожалуйста, введите название города")]
        public string City { get; set; }
        [Required(ErrorMessage ="Пожалуйста, введите название страны")]
        public string Country { get; set; }
        public bool GiftWrap { get; set; }
    }
}
