using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Models
{
    public class TrainingModel
    {
        public int Id { get; set; }
        public int? TrainerId { get; set; }
        public string? Trainer { get; set; }
        public int? RoomId { get; set; }
        public string? Room { get; set; }
        public string Name { get; set; }
        public DateTime TrainingStart { get; set; }
        public DateTime TrainingEnd { get; set; }
        public int MaxPeople { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
