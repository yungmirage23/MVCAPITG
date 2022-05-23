using Microsoft.AspNetCore.Http;

namespace RestWebAppl.Models.ViewModels
{
    public class UserDataViewModel
    {
        public ApplicationUser User { get; set; }
        public IQueryable<Order>? Orders { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NewPassword { get; set; }
        public string? OldPassword { get; set; }
        public string? Email { get; set; }
        public byte[]? UserPhoto { get; set; }
        public IFormFile? Avatar {get; set;}
    }
}
