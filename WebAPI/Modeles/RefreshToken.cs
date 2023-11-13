using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Modeles
{
    public class RefreshToken
    {
        [Key]
        public int ID { get; set; } 

        public string Token { get;set ; }

        public string JwtId { get; set; }   

        public DateTime CreatedDateTimeUtc { get; set; }      

        public DateTime ExpiryDateTimeUtc { get; set; }

        public bool Used { get; set; }  

        public bool Invalidated { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
