using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RestWebAppl.Models
{
    
    public class Item
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Пожалуйста, введите название товара")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите название категории")]
        public string Category { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите описание для товара")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Выберите цену товара")]
        public decimal Price { get; set; }
        public string addedTime { get; set; }
        public string? ImagePath { get; set; }
        
      
        
       
    }
}
