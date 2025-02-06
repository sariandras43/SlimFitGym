using System;
using System.Collections.Generic;
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
        public string AccountId { get; set; }

        [ForeignKey("Machines")]
        public string MachineId { get; set; }

        [ForeignKey("Rooms")]
        public string RoomId { get; set; }

        [Required, StringLength(200)]
        public string Url { get; set; }
    }
}
