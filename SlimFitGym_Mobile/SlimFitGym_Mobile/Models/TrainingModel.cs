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
        public int TrainerId { get; set; }
        public int RoomId { get; set; }
        public string Name { get; set; }
        public DateTime TrainingStart { get; set; }
        public DateTime TrainingEnd { get; set; }
        public int MaxPeople { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        public static List<TrainingModel> GetTrainings()
        {
            List<TrainingModel> trainings = new List<TrainingModel>();
            trainings.Add(new TrainingModel { Id = 1, TrainerId = 1, RoomId = 1, Name = "Crossfit", TrainingStart = new DateTime(2021, 6, 1, 10, 0, 0), TrainingEnd = new DateTime(2021, 6, 1, 11, 0, 0), MaxPeople = 10 });
            trainings.Add(new TrainingModel { Id = 2, TrainerId = 2, RoomId = 2, Name = "Yoga", TrainingStart = new DateTime(2021, 6, 1, 12, 0, 0), TrainingEnd = new DateTime(2021, 6, 1, 13, 0, 0), MaxPeople = 10 });
            trainings.Add(new TrainingModel { Id = 3, TrainerId = 3, RoomId = 3, Name = "Pilates", TrainingStart = new DateTime(2021, 6, 1, 14, 0, 0), TrainingEnd = new DateTime(2021, 6, 1, 15, 0, 0), MaxPeople = 10 });
            trainings.Add(new TrainingModel { Id = 4, TrainerId = 4, RoomId = 4, Name = "Zumba", TrainingStart = new DateTime(2021, 6, 1, 16, 0, 0), TrainingEnd = new DateTime(2021, 6, 1, 17, 0, 0), MaxPeople = 10 });
            trainings.Add(new TrainingModel { Id = 5, TrainerId = 5, RoomId = 5, Name = "Body Pump", TrainingStart = new DateTime(2021, 6, 1, 18, 0, 0), TrainingEnd = new DateTime(2021, 6, 1, 19, 0, 0), MaxPeople = 10 });
            return trainings;
        }
    }
}
