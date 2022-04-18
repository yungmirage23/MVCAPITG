using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace RestWebAppl.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PatronymicName { get; set; }
        public string? AdditionalPhone { get; set; }
        public byte[]? UserPhoto { get; set; }
    }
}
