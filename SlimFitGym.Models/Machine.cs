using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models
{
    [Table("Machines")]
    public class Machine
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100),Required]
        public string Name { get; set; }

        [MaxLength(500), AllowNull, DefaultValue(null)]
        public string? Description { get; set; } = null;
    }
}
