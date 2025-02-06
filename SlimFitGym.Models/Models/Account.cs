using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlimFitGym.Models.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100), Required]
        public string Name { get; set; }

        [StringLength(320), Required]
        public string Email { get; set; }

        [StringLength(13), Required]
        public string Phone { get; set; }

        [Required]
        public string Role { get; set; }

        [Required, DefaultValue(true)]
        public bool isActive { get; set; }
    }
}
