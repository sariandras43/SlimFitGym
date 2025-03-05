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
    [Table("Rooms")]
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100), Required]
        public required string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int RecommendedPeople { get; set; }

        [Required, DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
