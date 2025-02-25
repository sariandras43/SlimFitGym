using Microsoft.EntityFrameworkCore;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
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
        public readonly RoomsRepository roomsRepository;
        public readonly AccountRepository accountRepository;
        public readonly ReservationRepository reservationRepository;
        public TrainingsRepository(SlimFitGymContext slimFitGymContext,
            RoomsRepository roomsRepository,
            AccountRepository accountRepository,
            ReservationRepository reservationRepository)
        {
            this.context = slimFitGymContext;
            this.roomsRepository = roomsRepository;
            this.accountRepository = accountRepository;
            this.reservationRepository = reservationRepository;
        }

        public List<Training> GetAllTrainings()
        {
            return context.Set<Training>().ToList();
        }

        public List<TrainingResponse> GetActiveTrainings()
        {

            return context.Set<Training>().Where(t=> t.IsActive).Select(t=>new TrainingResponse()
            {
                Id=t.Id,
                Name=t.Name,
                MaxPeople=t.MaxPeople,
                IsActive=t.IsActive,
                TrainingStart = t.TrainingStart,
                TrainingEnd = t.TrainingEnd,
                Trainer = accountRepository.GetAccountById(t.TrainerId)!.Name,
                Room = roomsRepository.GetRoomById(t.RoomId)!.Name,
                FreePlaces = t.MaxPeople-reservationRepository.GetReservationsByTrainingId(t.Id)!.Count()

            }).ToList();
        }

        public List<TrainingResponse> GetTrainingsByAccountId(int accountId)
        {
            var reservations = reservationRepository.GetReservationsByAccountId(accountId).Select(r=>r.TrainingId).ToList();
            List<TrainingResponse?> res = new List<TrainingResponse>();
            foreach (int trainingId in reservations)
            {
            var traininRes = context.Set<Training>().Where(t => t.Id==trainingId).Select(t => new TrainingResponse()
            {
                Id = t.Id,
                Name = t.Name,
                MaxPeople = t.MaxPeople,
                IsActive = t.IsActive,
                TrainingStart = t.TrainingStart,
                TrainingEnd = t.TrainingEnd,
                Trainer = accountRepository.GetAccountById(t.TrainerId)!.Name,
                Room = roomsRepository.GetRoomById(t.RoomId)!.Name,
                FreePlaces = t.MaxPeople - reservationRepository.GetReservationsByTrainingId(t.Id)!.Count()

            }).SingleOrDefault();
            res.Add(traininRes);
            }
            return res;
        }

        public TrainingResponse? GetActiveTraningById(int id)
        {
            if (id <= 0)
                return null;
            Training? t = context.Set<Training>().ToList().SingleOrDefault(t => t.Id == id);
            if (t != null)
                return new TrainingResponse()
                {
                    Id = t.Id,
                    Name = t.Name,
                    MaxPeople = t.MaxPeople,
                    IsActive = t.IsActive,
                    TrainingStart = t.TrainingStart,
                    TrainingEnd = t.TrainingEnd,
                    Trainer = accountRepository.GetAccountById(t.TrainerId)!.Name,
                    Room = roomsRepository.GetRoomById(t.RoomId)!.Name,
                    FreePlaces = t.MaxPeople - reservationRepository.GetReservationsByTrainingId(t.Id)!.Count()

                };
            return null;
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

        public Training? NewTraining(TrainingRequest training)
        {
            if (training.TrainerId <= 0)
                throw new Exception("Ilyen edző nem létezik");
            if (training.RoomId <= 0)
                throw new Exception("Ilyen terem nem létezik");
            if (training == null)
                throw new Exception("Hibás kérés.");
            if (training.Name== null || training.Name.Length == 0)
                throw new Exception("A név mező kitöltése kötelező.");
            if (training.Description == null || training.Description.Length == 0)
                throw new Exception("A leírás mező kitöltése kötelező.");
            TimeSpan trainingSpan = training.TrainingEnd - training.TrainingStart; 
            if (trainingSpan.TotalMinutes<30)
                throw new Exception("Érvénytelen időintervallum (minimum 30 perces edzés vehető fel).");
            if (training.MaxPeople<1)
                throw new Exception("A maximum résztvevők mező kitöltése kötelező");
            if (training.Name.Length > 100)
                throw new Exception("A név maximum 100 karakter hosszú lehet.");
            if (training.Description.Length > 500)
                throw new Exception("A leírás maximum 500 karakter hosszú lehet.");
            Account? account = accountRepository.GetAccountById(training.TrainerId);
            if (account==null)
                throw new Exception("Ilyen fiók nem létezik");
            if (account.Role=="user")
                throw new Exception("Ez a felhasználó nem jogosult edzés felvételére");
            Room? room = roomsRepository.GetRoomById(training.RoomId);
            if (room == null)
                throw new Exception("Ilyen terem nem létezik");
            List<Training> trainingsInTheSpecificRoom = context.Set<Training>().Where(t=>t.RoomId==training.RoomId && t.IsActive).ToList();
            foreach (Training t in trainingsInTheSpecificRoom)
            {
                if (t.TrainingStart <= training.TrainingStart && t.TrainingEnd >= training.TrainingEnd)
                    throw new Exception("Ebben az időpontban foglalt a terem");
            }

            Training trainingToSave = new Training()
            {
                TrainingStart = training.TrainingStart,
                TrainingEnd = training.TrainingEnd,
                IsActive=true,
                Description =training.Description,
                Name = training.Name,
                MaxPeople=training.MaxPeople,
                RoomId = training.RoomId,
                TrainerId = training.TrainerId
            };

            Training savedTraining = this.context.Set<Training>().Add(trainingToSave).Entity;
            this.context.SaveChanges();
            return savedTraining;
        }

        public Training? UpdateTraining(int id,TrainingRequest training)
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
            if (training.Description == null || training.Description.Length == 0)
                throw new Exception("A leírás mező kitöltése kötelező.");
            TimeSpan trainingSpan = training.TrainingEnd - training.TrainingStart;
            if (trainingSpan.TotalMinutes < 30)
                throw new Exception("Érvénytelen időintervallum (minimum 30 perces edzés vehető fel).");
            if (training.MaxPeople < 1)
                throw new Exception("A maximum résztvevők mező kitöltése kötelező");
            if (training.Description.Length > 500)
                throw new Exception("A leírás maximum 500 karakter hosszú lehet.");
            if (training.Name.Length > 100)
                throw new Exception("A név maximum 100 karakter hosszú lehet.");
            if (training.MaxPeople < 1)
                throw new Exception("A maximum résztvevők mező kitöltése kötelező");
            if (training.Name.Length > 100)
                throw new Exception("A név maximum 100 karakter hosszú lehet.");
            if (!this.context.Set<Training>().Any(t => t.Id == id && t.IsActive))
                return null;
            if (training.MaxPeople - reservationRepository.GetReservationsByTrainingId(id)!.Count < 0)
                throw new Exception("Több a résztvevő, mint az új látszám");
            Account? account = accountRepository.GetAccountById(training.TrainerId);
            if (account == null)
                throw new Exception("Ilyen edző nem létezik");
            if (account.Role == "user")
                throw new Exception("Ez a felhasználó nem jogosult edzés felvételére");
            Room? room = roomsRepository.GetRoomById(training.RoomId);
            if (room == null)
                throw new Exception("Ilyen terem nem létezik");
            List<Training> trainingsInTheSpecificRoom = context.Set<Training>().AsNoTracking().Where(t => t.RoomId == training.RoomId && t.IsActive).ToList();
            trainingsInTheSpecificRoom.RemoveAll(t=>t.Id==id);
            foreach (Training t in trainingsInTheSpecificRoom)
            {
                if (t.TrainingStart <= training.TrainingStart && t.TrainingEnd >= training.TrainingEnd)
                    throw new Exception("Ebben az időpontban foglalt a terem");
            }

            Training trainingToModify = new Training()
            {
                Id = training.Id,
                TrainingStart = training.TrainingStart,
                TrainingEnd = training.TrainingEnd,
                Description = training.Description,
                IsActive = true,
                Name = training.Name,
                MaxPeople = training.MaxPeople,
                RoomId = training.RoomId,
                TrainerId = training.TrainerId
            };

            this.context.Entry(trainingToModify).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return trainingToModify;
        }

        public TrainingResponse? DeleteOrMakeInactive(int id)
        {
            if (id <= 0)
                throw new Exception("Érvénytelen azonosító.");
            Training? t = this.context.Set<Training>().SingleOrDefault(t => t.Id == id);
            if (t == null)
                return null;
            if (!t.IsActive)
                return null;

            if (reservationRepository.GetReservationsByTrainingId(id)!.Count==0)
            {
                this.context.Set<Training>().Remove(t);
                this.context.SaveChanges();
                return new TrainingResponse()
                {
                    Id = t.Id,
                    Name = t.Name,
                    MaxPeople = t.MaxPeople,
                    IsActive = t.IsActive,
                    TrainingStart = t.TrainingStart,
                    TrainingEnd = t.TrainingEnd,
                    Trainer = accountRepository.GetAccountById(t.TrainerId)!.Name,
                    Room = roomsRepository.GetRoomById(t.RoomId)!.Name,
                    FreePlaces = t.MaxPeople - reservationRepository.GetReservationsByTrainingId(t.Id)!.Count()
                };

            }
            else
            {
                t.IsActive = false;
                this.context.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                this.context.SaveChanges();
                return new TrainingResponse()
                {
                    Id = t.Id,
                    Name = t.Name,
                    MaxPeople = t.MaxPeople,
                    IsActive = t.IsActive,
                    TrainingStart = t.TrainingStart,
                    TrainingEnd = t.TrainingEnd,
                    Trainer = accountRepository.GetAccountById(t.TrainerId)!.Name,
                    Room = roomsRepository.GetRoomById(t.RoomId)!.Name,
                    FreePlaces = t.MaxPeople - reservationRepository.GetReservationsByTrainingId(t.Id)!.Count()
                };
            }
        }
    }
}
