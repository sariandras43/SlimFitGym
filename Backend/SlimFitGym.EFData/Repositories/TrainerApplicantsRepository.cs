﻿using SlimFitGym.EFData.Interfaces;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using SlimFitGymBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Repositories
{
    public class TrainerApplicantsRepository:ITrainerApplicantsRepository
    {
        readonly SlimFitGymContext context;
        readonly IAccountRepository accountRepository;
        readonly TokenGenerator tokenGenerator;
        public TrainerApplicantsRepository(SlimFitGymContext slimFitGymContext, IAccountRepository accountRepository, TokenGenerator tokenGenerator)
        {
            context = slimFitGymContext;
            this.accountRepository = accountRepository;
            this.tokenGenerator = tokenGenerator;
        }

        public List<TrainerApplicant> GetAllApplicants()
        {
            return context.Set<TrainerApplicant>().ToList();
        }

        public TrainerApplicant? GetApplicantById(int id)
        {
            if (id <= 0)
                return null;
            var res = context.Set<TrainerApplicant>().SingleOrDefault(t => t.Id == id);
            if (res != null)
                return res;
            return null;
        }

        public TrainerApplicant? NewApplicant(string token, TrainerApplicant applicant)
        {
            if (applicant.AccountId <= 0)
                throw new Exception("Nem létezik ez a felhasználó.");

            Account? a = context.Set<Account>().SingleOrDefault(a => a.Id == applicant.AccountId);
            if (a == null)
                return null;
            Account? accountFromToken = accountRepository.GetAccountById(tokenGenerator.GetAccountIdFromToken(token));
            if (accountFromToken == null)
                throw new Exception("Érvénytelen token.");
            if (a.Id != tokenGenerator.GetAccountIdFromToken(token) || accountFromToken.Id != applicant.AccountId)
                throw new Exception("Nem lehet más felhasználóként jelentkezni edzőnek.");


            if (context.Set<TrainerApplicant>().Any(ta => ta.AccountId == applicant.AccountId))
                throw new Exception("Ez a felhasználó már jelentkezett edzőnek");
            if (a.Role == "trainer" || a.Role == "admin")
                throw new Exception("Ez a felhasználó már edző.");
            TrainerApplicant savedApplicant = this.context.Set<TrainerApplicant>().Add(applicant).Entity;
            this.context.SaveChanges();
            return savedApplicant;
        }

        public AccountResponse? AcceptAsTrainer(int id)
        {
            if (id <= 0)
                return null;

            TrainerApplicant? tr = GetApplicantById(id);
            if (tr == null)
                return null;

            AccountResponse? newTrainer = accountRepository.BecomeATrainer(tr.AccountId);
            if (newTrainer == null)
                return null;
            this.context.Set<TrainerApplicant>().Remove(tr);
            this.context.SaveChanges();
            return newTrainer;
        }

        public AccountResponse? Reject(int id)
        {
            if (id <= 0)
                return null;

            TrainerApplicant? tr = GetApplicantById(id);
            if (tr == null)
                return null;

            Account? applicant = accountRepository.GetAccountById(tr.AccountId);
            if (applicant == null)
                return null;
            this.context.Set<TrainerApplicant>().Remove(tr);
            this.context.SaveChanges();
            return new AccountResponse(applicant);
        }
    }
}
