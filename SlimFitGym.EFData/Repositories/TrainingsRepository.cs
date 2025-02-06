using SlimFitGym.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Repositories
{
    public class TrainingsRepository
    {
        public readonly SlimFitGymContext context;
        public TrainingsRepository(SlimFitGymContext slimFitGymContext)
        {
            this.context = slimFitGymContext;
        }

        public List<Training> GetAllTrainings()
        {
            return context.Set<Training>().ToList();
        }
        public Training? GetTrainingById(int id)
        {
            if (id <= 0)
                return null;
            var res = context.Set<Training>().ToList().SingleOrDefault(t => t.Id == id);
            if (res != null)
                return res;
            return null;
        }

        public List<Training> FilterTrainings(string nameFragment,int limit, int offset)
        {
            if (limit<=0)
                limit = 20;
            if (offset<0)
                offset = 0;
            return context.Set<Training>().Where(t=>t.Name.ToLower().Contains(nameFragment.ToLower())).Skip(offset).Take(limit).ToList();
        }

        public Training? NewTraining(Training training)
        {
            if (training.TrainerId <= 0)
                throw new Exception("Ilyen edző nem létezik");
            if (training.RoomId <= 0)
                throw new Exception("Ilyen terem nem létezik");
            if (training == null)
                throw new Exception("Hibás kérés.");
            if (training.Name== null || training.Name.Length == 0)
                throw new Exception("A név mező kitöltése kötelező.");
            TimeSpan trainingSpan = training.TrainingEnd - training.TrainingStart; 
            if (trainingSpan.TotalMinutes<30)
                throw new Exception("Érvénytelen időintervallum (minimum 30 perces edzés vehető fel).");
            if (training.MaxPeople<1)
                throw new Exception("A maximum résztvevők mező kitöltése kötelező");
            if (training.Name.Length > 100)
                throw new Exception("A név maximum 100 karakter hosszú lehet.");
            Account? account = context.Set<Account>().SingleOrDefault(a => a.Id == training.TrainerId);
            Room? room = context.Set<Room>().SingleOrDefault(r=>r.Id == training.RoomId);
            if (account==null)
                throw new Exception("Ilyen fiók nem létezik");
            if (room == null)
                throw new Exception("Ilyen terem nem létezik");
            if (account.Role=="user")
                throw new Exception("Ez a felhasználó nem jogosult edzés felvételére");
            List<Training> trainingsInTheSpecificRoom = context.Set<Training>().Where(t=>t.RoomId==training.RoomId).ToList();
            foreach (Training t in trainingsInTheSpecificRoom)
            {
                if (t.TrainingStart <= training.TrainingStart && t.TrainingEnd >= training.TrainingEnd)
                    throw new Exception("Ebben az időpontban foglalt a terem");
            }

            Training savedTraining = this.context.Set<Training>().Add(training).Entity;
            this.context.SaveChanges();
            return savedTraining;
        }

        public Training? UpdateTraining(int id,Training training)
        {
            if (training.TrainerId<=0)
                throw new Exception("Ilyen edző nem létezik");
            if (training.RoomId <= 0)
                throw new Exception("Ilyen terem nem létezik");
            if (id != training.Id)
                throw new Exception("Érvénytelen azonosító.");
            if (training == null)
                throw new Exception("Hibás kérés.");
            if (training.Name == null || training.Name.Length == 0)
                throw new Exception("A név mező kitöltése kötelező.");
            if (training.TrainingEnd <= training.TrainingStart)
                throw new Exception("Érvénytelen időintervallum.");
            if (training.MaxPeople < 1)
                throw new Exception("A maximum résztvevők mező kitöltése kötelező");
            if (training.Name.Length > 100)
                throw new Exception("A név maximum 100 karakter hosszú lehet.");
            if (!this.context.Set<Training>().Any(t => t.Id == id))
                return null;
            Account? account = context.Set<Account>().SingleOrDefault(a => a.Id == training.TrainerId);
            Room? room = context.Set<Room>().SingleOrDefault(r => r.Id == training.RoomId);
            if (account == null)
                throw new Exception("Ilyen edző nem létezik");
            if (room == null)
                throw new Exception("Ilyen terem nem létezik");
            if (account.Role == "user")
                throw new Exception("Ez a felhasználó nem jogosult edzés felvételére");
            List<Training> trainingsInTheSpecificRoom = context.Set<Training>().Where(t => t.RoomId == training.RoomId).ToList();
            trainingsInTheSpecificRoom.Remove(training);
            foreach (Training t in trainingsInTheSpecificRoom)
            {
                if (t.TrainingStart <= training.TrainingStart && t.TrainingEnd >= training.TrainingEnd)
                    throw new Exception("Ebben az időpontban foglalt a terem");
            }

            this.context.Entry(training).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return training;
        }

        public Training? DeleteTraining(int id)
        {
            if (id <= 0)
                return null;
            var trainingToDelete = this.context.Set<Training>().SingleOrDefault(t => t.Id == id);
            if (trainingToDelete == null)
                return null;

            this.context.Set<Training>().Remove(trainingToDelete);
            this.context.SaveChanges();
            return trainingToDelete;
        }
    }
}
