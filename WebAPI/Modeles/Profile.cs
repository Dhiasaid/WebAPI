using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Modeles
{
    public class Profile
    {
        [Key]
        public int id { get; set; }

        public string UserId { get; set; }

        [StringLength(100)]
        public string Address1 { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }


        [StringLength(100)]
        public string city{ get; set; }


        [StringLength(100)]
        public string state { get; set; }


        [StringLength(100)]
        public string landmark { get; set; }


        [StringLength(100)]
        public string Pin { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }


    }
}
