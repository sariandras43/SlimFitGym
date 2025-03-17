using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Models
{
    [Table("TrainerApplicants")]
    public class TrainerApplicant
    {
        [Key]
        public int Id { get; set; }
        [Required,ForeignKey("Accounts")]
        public int AccountId { get; set; }
        public DateTime? AcceptedAt { get; set; }
    }
}
