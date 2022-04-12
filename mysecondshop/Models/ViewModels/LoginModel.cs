using System.ComponentModel.DataAnnotations;
namespace mysecondshop.Models.ViewModels
{
    public class LoginModel
    {
        [Required (AllowEmptyStrings = false, ErrorMessage ="Не указан номер телефона")]
        public string Phone { get; set; }
        [Required (AllowEmptyStrings = false, ErrorMessage = "Не указан пароль")]
        [StringLength (15)]
        [UIHint("password")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
