using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Models
{
    [Table("Benefits")]
    public class Benefit
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string BenefitName { get; set; }

    }
}
