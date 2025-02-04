using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models
{
    [Table("Entries")]
    public class Entry
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Accounts")]
        public string AccountId { get; set; }

        [Required]
        public DateTime EntryDate { get; set; }
    }
}
