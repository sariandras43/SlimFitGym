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
    public class Training
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey("Accounts")]
        public int TrainerId { get; set; }

        [Required, ForeignKey("Rooms")]
        public int RoomId { get; set; }

        [Required, StringLength(100)]
        public string Name{ get; set; }

        [Required]
        public DateTime TrainingStart { get; set; }

        [Required]
        public DateTime TrainingEnd { get; set; }

        [Required]
        public int MaxPeople { get; set; }

        [Required, DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
