﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SlimFitGym.EFData.Interfaces;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using SlimFitGymBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Repositories
{
    public class TrainingsRepository: ITrainingsRepository
    {
        public readonly SlimFitGymContext context;
        public readonly IRoomsRepository roomsRepository;
        public readonly IAccountRepository accountRepository;
        public readonly IReservationRepository reservationRepository;
        public readonly IImagesRepository imagesRepository;
        public readonly TokenGenerator tokenGenerator;
        public TrainingsRepository(SlimFitGymContext slimFitGymContext,
            IRoomsRepository roomsRepository,
            IAccountRepository accountRepository,
            IReservationRepository reservationRepository,
            IImagesRepository imagesRepository,
            TokenGenerator tokenGenerator) 
        {
            this.context = slimFitGymContext;
            this.roomsRepository = roomsRepository;
            this.accountRepository = accountRepository;
            this.reservationRepository = reservationRepository;
            this.imagesRepository = imagesRepository;
            this.tokenGenerator = tokenGenerator;
        }

        public List<TrainingResponse> GetAllTrainings()
        {
            return context.Set<Training>().Select(t => new TrainingResponse()
            {
                Id = t.Id,
                Name = t.Name,
                MaxPeople = t.MaxPeople,
                IsActive = t.IsActive,
                TrainingStart = t.TrainingStart.ToUniversalTime(),
                TrainingEnd = t.TrainingEnd.ToUniversalTime(),
                Trainer = accountRepository.GetAccountById(t.TrainerId)!.Name,
                Room = roomsRepository.GetRoomById(t.RoomId)!.Name,
                FreePlaces = t.MaxPeople - reservationRepository.GetReservationsByTrainingId(t.Id)!.Count(),
                TrainerImageUrl = imagesRepository.GetImageUrlByAccountId(t.TrainerId),
                RoomImageUrl = imagesRepository.GetImageUrlByRoomId(t.RoomId),
                TrainerId = t.TrainerId,
                RoomId = t.RoomId

            }).ToList();
        }

        public List<TrainingResponse> GetActiveTrainings(string query = "", int limit = 20, int offset = 0)
        {
            List<TrainingResponse> trainings = 
            trainings = context.Set<Training>().Where(t=> t.IsActive && t.TrainingStart > DateTime.UtcNow).Select(t=>new TrainingResponse()
            {
                Id=t.Id,
                Name=t.Name,
                MaxPeople=t.MaxPeople,
                IsActive=t.IsActive,
                TrainingStart = t.TrainingStart.ToUniversalTime(),
                TrainingEnd = t.TrainingEnd.ToUniversalTime(),
                Trainer = accountRepository.GetAccountById(t.TrainerId)!.Name,
                Room = roomsRepository.GetRoomById(t.RoomId)!.Name,
                FreePlaces = t.MaxPeople-reservationRepository.GetReservationsByTrainingId(t.Id)!.Count(),
                TrainerImageUrl = imagesRepository.GetImageUrlByAccountId(t.TrainerId),
                RoomImageUrl = imagesRepository.GetImageUrlByRoomId(t.RoomId),
                TrainerId = t.TrainerId,
                RoomId = t.RoomId

            }).ToList();

            return trainings.Where(t => t.Name.ToLower().Contains(query.Trim().ToLower()) || t.Trainer.ToLower().Contains(query.Trim().ToLower())|| t.Room.ToLower().Contains(query.Trim().ToLower())).Skip(offset).Take(limit).ToList();
        }
        public int GetTotalTrainingCountFromNow()
        {
            return context.Set<Training>().Where(t=>t.IsActive && t.TrainingStart > DateTime.UtcNow).Count();
        }

        public List<TrainingResponse>? GetTrainingsByAccountId(string token, int accountId)
        {
            Account? accountFromToken = accountRepository.GetAccountById(tokenGenerator.GetAccountIdFromToken(token));
            if (accountFromToken == null)
                throw new Exception("Érvénytelen token.");
            Account? account = accountRepository.GetAccountById(accountId);
            if (accountId != tokenGenerator.GetAccountIdFromToken(token) && accountFromToken.Role != "admin")
                throw new UnauthorizedAccessException();
            if (account == null) return null;
            List<int> reservations = reservationRepository.GetReservationsByAccountId(accountId).Select(r=>r.TrainingId).ToList();
            List<TrainingResponse> res = new List<TrainingResponse>();
            foreach (int trainingId in reservations)
            {
                var traininRes = context.Set<Training>().Where(t => t.Id==trainingId && t.TrainingStart > DateTime.UtcNow).Select(t => new TrainingResponse()
                {
                    Id = t.Id,
                    Name = t.Name,
                    MaxPeople = t.MaxPeople,
                    IsActive = t.IsActive,
                    TrainingStart = t.TrainingStart.ToUniversalTime(),
                    TrainingEnd = t.TrainingEnd.ToUniversalTime(),
                    Trainer = accountRepository.GetAccountById(t.TrainerId)!.Name,
                    Room = roomsRepository.GetRoomById(t.RoomId)!.Name,
                    FreePlaces = t.MaxPeople - reservationRepository.GetReservationsByTrainingId(t.Id)!.Count(),
                    TrainerImageUrl = imagesRepository.GetImageUrlByAccountId(t.TrainerId),
                    RoomImageUrl = imagesRepository.GetImageUrlByRoomId(t.RoomId),
                    TrainerId = t.TrainerId,
                    RoomId = t.RoomId,

                }).SingleOrDefault();
                if (traininRes !=null)
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
                    TrainingStart = t.TrainingStart.ToUniversalTime(),
                    TrainingEnd = t.TrainingEnd.ToUniversalTime(),
                    Trainer = accountRepository.GetAccountById(t.TrainerId)!.Name,
                    Room = roomsRepository.GetRoomById(t.RoomId)!.Name,
                    FreePlaces = t.MaxPeople - reservationRepository.GetReservationsByTrainingId(t.Id)!.Count(),
                    TrainerImageUrl = imagesRepository.GetImageUrlByAccountId(t.TrainerId),
                    RoomImageUrl = imagesRepository.GetImageUrlByRoomId(t.RoomId),
                    RoomId = t.RoomId,
                    TrainerId = t.TrainerId,

                };
            return null;
        }


        public List<TrainingResponse>? GetActiveTrainingsByRoomId(int roomId)
        {
            if (roomId <= 0)
                throw new Exception("Érvénytelen azonosító.");
            Room? room = this.context.Set<Room>().SingleOrDefault(r=>r.Id == roomId && r.IsActive);
            if (room == null)
                return null;
            List<TrainingResponse>? trainings = this.context.Set<Training>().Where(t=>t.RoomId== roomId && t.IsActive && t.TrainingStart>DateTime.UtcNow).Select(t=>new TrainingResponse()
            {
                Id = t.Id,
                Name = t.Name,
                MaxPeople = t.MaxPeople,
                IsActive = t.IsActive,
                TrainingStart = t.TrainingStart.ToUniversalTime(),
                TrainingEnd = t.TrainingEnd.ToUniversalTime(),
                Trainer = accountRepository.GetAccountById(t.TrainerId)!.Name,
                Room = room.Name,
                FreePlaces = t.MaxPeople - reservationRepository.GetReservationsByTrainingId(t.Id)!.Count(),
                TrainerImageUrl = imagesRepository.GetImageUrlByAccountId(t.TrainerId),
                RoomImageUrl = imagesRepository.GetImageUrlByRoomId(t.RoomId),
                TrainerId = t.TrainerId,
                RoomId = t.RoomId
            }).ToList();
            return trainings;
        }

        public List<TrainingResponse>? GetActiveTrainingsByTrainerId(int trainerId)
        {
            if (trainerId <= 0)
                throw new Exception("Érvénytelen azonosító.");
            Account? account = this.context.Set<Account>().SingleOrDefault(a => a.Id == trainerId && (a.Role=="trainer" || a.Role == "admin") && a.isActive);
            if (account == null)
                return null;
            List<TrainingResponse>? trainings = this.context.Set<Training>().Where(t => t.TrainerId == trainerId && t.IsActive && t.TrainingStart > DateTime.UtcNow).Select(t => new TrainingResponse()
            {
                Id = t.Id,
                Name = t.Name,
                MaxPeople = t.MaxPeople,
                IsActive = t.IsActive,
                TrainingStart = t.TrainingStart.ToUniversalTime(),
                TrainingEnd = t.TrainingEnd.ToUniversalTime(),
                Trainer = account.Name,
                Room = roomsRepository.GetRoomById(t.RoomId)!.Name,
                FreePlaces = t.MaxPeople - reservationRepository.GetReservationsByTrainingId(t.Id)!.Count(),
                TrainerImageUrl = imagesRepository.GetImageUrlByAccountId(t.TrainerId),
                RoomImageUrl = imagesRepository.GetImageUrlByRoomId(t.RoomId),
                TrainerId = t.TrainerId,
                RoomId = t.RoomId
            }).ToList();
            return trainings;
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

        public Training? NewTraining(string token, TrainingRequest training)
        {
            if (training == null)
                throw new Exception("Hibás kérés.");
            if (training.TrainerId <= 0)
                throw new Exception("Ilyen edző nem létezik");
            if (training.RoomId <= 0)
                throw new Exception("Ilyen terem nem létezik");
            Account? accountFromToken = accountRepository.GetAccountById(tokenGenerator.GetAccountIdFromToken(token));
            if (accountFromToken == null)
                throw new Exception("Érvénytelen token.");
            if (training.TrainerId != tokenGenerator.GetAccountIdFromToken(token) ||  accountFromToken.Id != training.TrainerId)
                throw new Exception("Nem lehet máshoz edzést felvenni.");
            if (training.Name== null || training.Name.Length == 0 || training.Name.Length > 100)
                throw new Exception("A név mező minimum 4, maximum 100 karakter hosszú lehet.");
            if (training.MaxPeople<1)
                throw new Exception("A maximum résztvevők mező kitöltése kötelező");
            //These arent neccesary
            Account? account = accountRepository.GetAccountById(training.TrainerId);
            if (account==null)
                throw new Exception("Ilyen fiók nem létezik");
            if (account.Role=="user")
                throw new Exception("Ez a felhasználó nem jogosult edzés felvételére");
            //
            Room? room = roomsRepository.GetRoomById(training.RoomId);
            if (room == null)
                throw new Exception("Ilyen terem nem létezik");
            if (training.TrainingStart<DateTime.UtcNow || training.TrainingEnd<DateTime.UtcNow)
               throw new Exception("Nem lehet múltba edzést felvenni.");
            TimeSpan trainingSpan = training.TrainingEnd - training.TrainingStart; 
            if (trainingSpan.TotalMinutes<30)
                throw new Exception("Érvénytelen időintervallum (minimum 30 perces edzés vehető fel).");
            //Any
            List<Training> trainingsInTheSpecificRoom = context.Set<Training>().AsNoTracking().Where(t=>t.RoomId==training.RoomId && t.IsActive).ToList();
            foreach (Training t in trainingsInTheSpecificRoom)
            {
                if (t.TrainingStart <= training.TrainingStart && t.TrainingEnd >= training.TrainingEnd)
                    throw new Exception("Ebben az időpontban foglalt a terem");
            }

            Training trainingToSave = new Training()
            {
                TrainingStart = training.TrainingStart.ToUniversalTime(),
                TrainingEnd = training.TrainingEnd.ToUniversalTime(),
                IsActive=true,
                Name = training.Name,
                MaxPeople=training.MaxPeople,
                RoomId = training.RoomId,
                TrainerId = training.TrainerId
            };

            Training savedTraining = this.context.Set<Training>().Add(trainingToSave).Entity;
            this.context.SaveChanges();
            return savedTraining;
        }

        public Training? UpdateTraining(string token, int id, TrainingRequest training)
        {
            if (training == null)
                throw new Exception("Hibás kérés.");
            if (id != training.Id)
                throw new Exception("Érvénytelen azonosító.");
            Account? accountFromToken = accountRepository.GetAccountById(tokenGenerator.GetAccountIdFromToken(token));
            if (accountFromToken == null)
                throw new Exception("Érvénytelen token.");
            Training? trainingToModify = context.Set<Training>().SingleOrDefault(t=>t.Id == id && t.IsActive);
            if (trainingToModify==null)
                return null;
            if (trainingToModify.TrainerId != tokenGenerator.GetAccountIdFromToken(token))
                throw new Exception("Nem lehet más edzését módosítani, felvenni.");
            if (DateTime.UtcNow > trainingToModify.TrainingStart)
                throw new Exception("Nem lehet már megtörtént edzés adatait módosítani.");

            List<Training> trainingsInTheSpecificRoom;
            if (training.RoomId != 0)
            {
                Room? room = roomsRepository.GetRoomById(training.RoomId);
                if (room == null)
                    throw new Exception("Ilyen terem nem létezik");
                trainingsInTheSpecificRoom = context.Set<Training>().AsNoTracking().Where(t => t.RoomId == training.RoomId && t.IsActive).ToList();
                trainingsInTheSpecificRoom.RemoveAll(t => t.Id == id);
                foreach (Training t in trainingsInTheSpecificRoom)
                {
                    if (t.TrainingStart <= training.TrainingStart && t.TrainingEnd >= training.TrainingEnd)
                        throw new Exception("Ebben az időpontban foglalt a terem");
                }
                trainingToModify.RoomId = training.RoomId;
            }

            if (training.Name!=null)
            {
                if (training.Name.Length < 4 || training.Name.Length>100)
                    throw new Exception("A név mező minimum 4, maximum 100 karakter hosszú lehet.");
                trainingToModify.Name = training.Name;
            }
            if (training.MaxPeople!=0)
            {
                if (training.MaxPeople < 1)
                    throw new Exception("A maximum résztvevők mező pozitív egész szám lehet.");
                if (training.MaxPeople - reservationRepository.GetReservationsByTrainingId(id)!.Count < 0)
                    throw new Exception("Több a résztvevő, mint az új látszám");
                trainingToModify.MaxPeople = training.MaxPeople;
            }

            if (training.TrainingStart>DateTime.UtcNow && training.TrainingEnd>DateTime.UtcNow)
            {
                TimeSpan trainingSpan = training.TrainingEnd - training.TrainingStart;
                if (trainingSpan.TotalMinutes < 30)
                    throw new Exception("Érvénytelen időintervallum (minimum 30 perces edzés vehető fel).");
                trainingsInTheSpecificRoom = context.Set<Training>().AsNoTracking().Where(t => t.RoomId == trainingToModify.RoomId && t.IsActive).ToList();
                trainingsInTheSpecificRoom.RemoveAll(t=>t.Id==id);
                //Any
                foreach (Training t in trainingsInTheSpecificRoom)
                {
                    if (t.TrainingStart <= training.TrainingStart && t.TrainingEnd >= training.TrainingEnd)
                        throw new Exception("Ebben az időpontban foglalt a terem");
                }
                trainingToModify.TrainingStart = training.TrainingStart.ToUniversalTime();
                trainingToModify.TrainingEnd = training.TrainingEnd.ToUniversalTime();
            }

            this.context.Entry(trainingToModify).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return trainingToModify;
        }

        public TrainingResponse? DeleteOrMakeInactive(string token, int id)
        {
            if (id <= 0)
                throw new Exception("Érvénytelen azonosító.");
            Account? accountFromToken = accountRepository.GetAccountById(tokenGenerator.GetAccountIdFromToken(token));
            Training? t = this.context.Set<Training>().SingleOrDefault(t => t.Id == id && t.IsActive);
            if (t == null)
                return null;
            if (accountFromToken == null)
                throw new Exception("Érvénytelen token.");
            if (accountFromToken.Role != "admin" && (t.TrainerId != tokenGenerator.GetAccountIdFromToken(token) || accountFromToken.Id != t.TrainerId))
                throw new Exception("Nem lehet más edzését törölni.");

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
                    TrainingStart = t.TrainingStart.ToUniversalTime(),
                    TrainingEnd = t.TrainingEnd.ToUniversalTime(),
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
                    TrainingStart = t.TrainingStart.ToUniversalTime(),
                    TrainingEnd = t.TrainingEnd.ToUniversalTime(),
                    Trainer = accountRepository.GetAccountById(t.TrainerId)!.Name,
                    Room = roomsRepository.GetRoomById(t.RoomId)!.Name,
                    FreePlaces = t.MaxPeople - reservationRepository.GetReservationsByTrainingId(t.Id)!.Count()
                };
            }
        }
    }
}
