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
    [Table("Images")]
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Accounts")]
        public int AccountId { get; set; }

        [ForeignKey("Machines")]
        public int MachineId { get; set; }

        [ForeignKey("Rooms")]
        public int RoomId { get; set; }

        [Required, StringLength(1024)]
        public string Url { get; set; }

        [Required, StringLength(255)]
        public required string CloudinaryId { get; set; }

    }
}
