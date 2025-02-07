using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string MachineId { get; set; }
        public string RoomId { get; set; }
        public string Url { get; set; }
    }
}
