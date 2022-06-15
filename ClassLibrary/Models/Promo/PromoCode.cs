using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class PromoCode
    {   [Key]
        [Required]
        [MaxLength(5)]
        public string Code { get; set; }
        public bool IsActive { get; set; }=false;
        public string? Email { get; set; } 
        
    }
}
