using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Requests
{
    public class TrainingRequest
    {
        public int Id { get; set; }
        public int TrainerId { get; set; }
        public int RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime TrainingStart { get; set; }
        public DateTime TrainingEnd { get; set; }
        public int MaxPeople { get; set; }
    }
}
