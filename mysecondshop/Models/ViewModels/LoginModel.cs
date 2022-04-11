using System.ComponentModel.DataAnnotations;
namespace mysecondshop.Models.ViewModels
{
    public class LoginModel
    {
        [Required (AllowEmptyStrings = false, ErrorMessage ="Не указан номер телефона")]
        public string Name { get; set; }
        [Required (AllowEmptyStrings = false, ErrorMessage = "Не указан пароль")]
        [UIHint("password")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
