using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Models
{
    [Table("RoomsAndMachines")]
    public class RoomAndMachine
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey("Rooms")]
        public int RoomId { get; set; }

        [Required, ForeignKey("Machines")]
        public int MachineId { get; set; }

        [Required]
        public int MachineCount { get; set; }
        public Room? Room { get; set; }
        public Machine? Machine { get; set; }
    }
}
