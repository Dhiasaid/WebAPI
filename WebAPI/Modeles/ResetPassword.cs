using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Modeles
{
    public class ResetPassword
    {
        [Key]
        public int Id { get; set; }

        [StringLength(450)]
        public string UserId { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(5000)]
        public string Token { get; set; }

        [StringLength(10)]
        public string OTP { get; set; }

        public DateTime InsetDateTimeUTC { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

    }
        
}
