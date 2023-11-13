using Microsoft.AspNetCore.Identity;

namespace WebAPI.Modeles
{
    public class ApplicationUser : IdentityUser
    {
        public static string? Admin = "Admin";
        public static string? User = "User";
        // public string IdDevice {get; set;}
        // public byte {} ProfilePicture {get; set;}
        public string CIN { get; set; }
    }
}
