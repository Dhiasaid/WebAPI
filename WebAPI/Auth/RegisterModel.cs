using System.ComponentModel.DataAnnotations;
namespace WebAPIl.Auth
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }
        // public string CIN {get; set;}
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Role { get;set; }
        public string  Address { get; set; }
        // public string IdDevice { get; set;}
        // public string ContentType { get; set;}
        // public string FileContent { get; set;}
        // public string ProfilePicture { get; set;}

        public string? CIN { get; set; } 
        public DateTime DateNais { get; set; }  

    }

}
