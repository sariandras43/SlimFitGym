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

        public int RoomId { get; set; }

        public int MachineId { get; set; }
        public int MachineCount { get; set; }
        public Room? Room { get; set; }
        public Machine? Machine { get; set; }
    }
}
