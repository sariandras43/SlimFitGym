using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Models
{
    [Table("PassesAndBenefits")]
    public class PassAndBenefit
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey("Passes")]
        public int PassId { get; set; }

        [Required,ForeignKey("Benefits")]
        public int BenefitId { get; set; }
        public Pass Pass { get; set; }
        public Benefit Benefit { get; set; }
    }
}
