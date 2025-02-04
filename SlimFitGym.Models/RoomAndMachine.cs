using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models
{
    [Table("RoomsAndMachines")]
    public class RoomAndMachine
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Rooms")]
        public int RoomId { get; set; }

        [ForeignKey("Machines")]
        public int MachineID { get; set; }
    }
}
