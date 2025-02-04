using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models
{
    [Table("Purchases")]
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required, ForeignKey("Passes")]
        public int PassId { get; set; }

        [Required, ForeignKey("Accounts")]
        public int AccountId { get; set; }
    }
}
