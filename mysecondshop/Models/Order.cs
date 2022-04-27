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
        public string FullName { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите название страны")]
        public string PhoneNumber { get; set; }
        public string? DeliveryAdress { get; set; }
        public string? DeliveryDistrict { get; set; }
        
        public bool Cash { get; set; }
        public bool SelfDeliver { get; set; }
    }
}
