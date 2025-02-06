using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Models
{
    [Table("Passes")]
    public class Pass
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int MaxEntries { get; set; }

        [Required]
        public int Days { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required, DefaultValue(true)]
        public bool IsActive { get; set; }

    }
}
