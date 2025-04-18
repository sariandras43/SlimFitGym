﻿using Microsoft.Extensions.DependencyInjection;
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
    public class PassesRepository:IPassesRepository
    {

        readonly SlimFitGymContext context;
        readonly IPurchasesRepository purchasesRepository;
        readonly IAccountRepository accountRepository;
        readonly TokenGenerator tokenGenerator;
        readonly IServiceProvider provider;
        public PassesRepository(SlimFitGymContext context,IPurchasesRepository purchasesRepository, IAccountRepository accountRepository, TokenGenerator tokenGenerator, IServiceProvider provider)
        {
            this.context = context;
            this.purchasesRepository = purchasesRepository;
            this.accountRepository = accountRepository;
            this.tokenGenerator = tokenGenerator;
            this.provider = provider;
        }

        private IEntriesRepository GetEntriesRepository()
            => provider.GetRequiredService<IEntriesRepository>();

        public List<PassResponse> GetAllActivePasses()
        {
            List<PassResponse> passes = context.Set<Pass>().Where(p=>p.IsActive).Select(p=>new PassResponse(p)).ToList();

            foreach (PassResponse p in passes)
            {
                List<PassAndBenefit> pbs = context.Set<PassAndBenefit>().Where(pb=>pb.PassId==p.Id).ToList();
                foreach (PassAndBenefit pb in pbs)
                {
                    p.Benefits.Add(context.Set<Benefit>().SingleOrDefault(b=>b.Id==pb.BenefitId).BenefitName);              
                }
            }
            return passes;
        }

        public List<PassResponse> GetAllPasses()
        {
            List<PassResponse> passes = context.Set<Pass>().Select(p => new PassResponse(p)).ToList();

            foreach (PassResponse p in passes)
            {
                List<PassAndBenefit> pbs = context.Set<PassAndBenefit>().Where(pb => pb.PassId == p.Id).ToList();
                foreach (PassAndBenefit pb in pbs)
                {
                    p.Benefits.Add(context.Set<Benefit>().SingleOrDefault(b => b.Id == pb.BenefitId).BenefitName);
                }
            }
            return passes;
        }

        public PassResponse? GetOnlyActivePassById(int id)
        {
            PassResponse? pass = context.Set<Pass>().Where(p => p.IsActive && p.Id == id).Select(p => new PassResponse(p)).SingleOrDefault();
            if (pass == null)
                return null;

            List<PassAndBenefit> pbs = context.Set<PassAndBenefit>().Where(pb => pb.PassId == pass.Id).ToList();
            foreach (PassAndBenefit pb in pbs)
            {
                pass.Benefits.Add(context.Set<Benefit>().SingleOrDefault(b => b.Id == pb.BenefitId).BenefitName);

            }

            return pass;
        }

        public PassResponse? GetPassById(int id)
        {
            PassResponse? pass = context.Set<Pass>().Where(p =>p.Id == id).Select(p => new PassResponse(p)).SingleOrDefault();
            if (pass == null)
                return null;
            if (!pass.isActive)
                return null;
            List<PassAndBenefit> pbs = context.Set<PassAndBenefit>().Where(pb => pb.PassId == pass.Id).ToList();
            foreach (PassAndBenefit pb in pbs)
            {
                pass.Benefits.Add(context.Set<Benefit>().SingleOrDefault(b => b.Id == pb.BenefitId).BenefitName);
            }

            return pass;
        }


        public Pass? GetPassModelById(int id)
        {
            Pass? pass = context.Set<Pass>().SingleOrDefault(p => p.Id == id);
            if (pass == null)
                return null;
            return pass;
        }


        public LatestPassResponse? GetLatestPassByAccountId(string token, int accountId)
        {
            IEntriesRepository entriesRepository = GetEntriesRepository();
            Account? accountFromToken = accountRepository.GetAccountById(tokenGenerator.GetAccountIdFromToken(token));
            if (accountFromToken == null)
                throw new Exception("Érvénytelen token.");
            if (accountId != tokenGenerator.GetAccountIdFromToken(token) && accountFromToken.Role != "admin")
                throw new UnauthorizedAccessException();
            if (accountId<=0)
                throw new Exception("Érvénytelen azonosító.");
            Account? account = accountRepository.GetAccountById(accountId);
            if (account == null) return null;
            Purchase? latestPurchase =  purchasesRepository.GetLatestPurchaseByAccountId(token, accountId);
            if (latestPurchase == null) return null;
            Pass? p = this.GetPassModelById(latestPurchase.PassId);
            // In theory this is unnecessary
            //if (p == null) return null;
            string purchaseDateInString = latestPurchase.PurchaseDate.Year.ToString()
                                            + '.' + latestPurchase.PurchaseDate.Month.ToString() 
                                            + '.' + latestPurchase.PurchaseDate.Day.ToString();
            List<Entry> entriesFromPurchase = entriesRepository.GetEntriesByAccountId(token, accountId, latestPurchase.PurchaseDate.ToString(),p.MaxEntries);
            if (p.Days!=0 &&p.MaxEntries!=0)
                return new LatestPassResponse(new PassResponse(p),latestPurchase.PurchaseDate.AddDays(p.Days), p.MaxEntries-entriesFromPurchase.Count());          
            else if(p.Days==0)
                return new LatestPassResponse(new PassResponse(p), null, p.MaxEntries - entriesFromPurchase.Count());
            else
                return new LatestPassResponse(new PassResponse(p), latestPurchase.PurchaseDate.AddDays(p.Days), null);



        }
        public PassResponse? NewPass(PassRequest pass)
        {
            if (pass==null)
                return null;
            if (pass.Price < 0)
                throw new Exception("Érvénytelen ár.");
            if (pass.Name.Length > 100)
                throw new Exception("Túl hosszú név.");
            if (pass.MaxEntries!=null && pass.Days!=null && pass.MaxEntries < 1 && pass.Days<1)
                throw new Exception("Kötelező megadni legalább a maximum belépések számát vagy a felhasználható napok értékét.");
            if (context.Set<Pass>().Any(p=>p.Name==pass.Name&&p.IsActive))
                throw new Exception("Ilyen névvel létezik már aktív bérlet.");


            Pass passToSave = new Pass()
            {
                Days = pass.Days,
                IsActive = true,
                IsHighlighted = pass.isHighlighted,
                MaxEntries = pass.MaxEntries,
                Name = pass.Name,
                Price = pass.Price
                
            };
            if (pass.isHighlighted==null)
                passToSave.IsHighlighted = false;
            Pass savedPass = this.context.Set<Pass>().Add(passToSave).Entity;

            this.context.SaveChanges();

            pass.Benefits.RemoveAll(s => string.IsNullOrWhiteSpace(s));
            foreach (string bName in pass.Benefits)
            {
                Benefit? b = context.Set<Benefit>().SingleOrDefault(b => b.BenefitName == bName);
                if (b==null)
                {
                    Benefit savedBenefit = this.context.Set<Benefit>().Add(new Benefit() { BenefitName=bName}).Entity;
                    this.context.SaveChanges();
                    PassAndBenefit newPb = this.context.Set<PassAndBenefit>().Add(new PassAndBenefit() { BenefitId=savedBenefit.Id,PassId=savedPass.Id}).Entity;
                    this.context.SaveChanges();
                }
                else
                {
                    PassAndBenefit? pb = this.context.Set<PassAndBenefit>().SingleOrDefault(pb=>pb.PassId==savedPass.Id && pb.BenefitId == b.Id);
                    if (pb==null)
                    {
                        PassAndBenefit newPb = this.context.Set<PassAndBenefit>().Add(new PassAndBenefit() { BenefitId = b.Id, PassId = savedPass.Id }).Entity;
                        this.context.SaveChanges();
      
                    }
                }                      
            }


            return GetPassById(savedPass.Id);
        }

        public PassResponse? UpdatePass(int id, PassRequest pass)
        {

            if (pass.Id <= 0)
                throw new Exception("Ilyen bérlet nem létezik");
            if (id != pass.Id)
                throw new Exception("Érvénytelen azonosító.");
            if (pass == null)
                throw new Exception("Hibás kérés.");
            Pass? p = context.Set<Pass>().SingleOrDefault(p=>p.Id == id && p.IsActive);
            if (p == null)
                throw new Exception("Nem található ilyen bérlet.");
            if (pass.Name!=null)
            {
                if (context.Set<Pass>().Any(p => p.Name == pass.Name && p.IsActive && pass.Name!=p.Name))
                    throw new Exception("Ilyen névvel létezik már aktív bérlet.");
                if (pass.Name.Length > 100 || pass.Name.Length<4)
                    throw new Exception("A név maximum 100, minimum 4 karakter lehet.");
                p.Name = pass.Name;
            }
            if (pass.Price!=0)
            {
                if (pass.Price < 0)
                    throw new Exception("Érvénytelen ár.");
                p.Price = pass.Price;
                
            }
            if (pass.isHighlighted!=null)
            {
                if (p.IsHighlighted!=pass.isHighlighted)
                    p.IsHighlighted = !p.IsHighlighted;      
            }
            if (pass.MaxEntries!=0 || pass.Days!=0)
            {
                if (pass.MaxEntries<1&& pass.Days<1)
                        throw new Exception("Kötelező megadni legalább a maximum belépések számát vagy a felhasználható napok értékét.");
                p.Days = pass.Days;
                p.MaxEntries = pass.MaxEntries;
                
            }
                
            
            

            if (this.purchasesRepository.GetAllPurchases().Any(purchase=>purchase.PassId==id))
            {
                DeleteOrMakePassInactive(id);
                return NewPass(pass);
            }
            this.context.Entry(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            if (pass.Benefits!=null && pass.Benefits.Count>0)
            {
                pass.Benefits.RemoveAll(s => string.IsNullOrWhiteSpace(s));
                foreach (string bName in pass.Benefits)
                {
                    Benefit? b = context.Set<Benefit>().SingleOrDefault(b => b.BenefitName == bName);
                    if (b == null)
                    {
                        Benefit savedBenefit = this.context.Set<Benefit>().Add(new Benefit() { BenefitName = bName }).Entity;
                        this.context.SaveChanges();
                        PassAndBenefit newPb = this.context.Set<PassAndBenefit>().Add(new PassAndBenefit() { BenefitId = savedBenefit.Id, PassId = pass.Id }).Entity;
                        this.context.SaveChanges();
                    }
                    else
                    {
                        PassAndBenefit? pb = this.context.Set<PassAndBenefit>().SingleOrDefault(pb => pb.PassId == pass.Id && pb.BenefitId == b.Id);
                        if (pb==null)
                        {
                            PassAndBenefit newPb = this.context.Set<PassAndBenefit>().Add(new PassAndBenefit() { BenefitId = b.Id, PassId = pass.Id }).Entity;
                            this.context.SaveChanges();
         
                        }

                    }
                }
                List<PassAndBenefit> passAndBenefits = context.Set<PassAndBenefit>().Where(pb=>pb.PassId==id).ToList();
                foreach (PassAndBenefit pb in passAndBenefits)
                {
                    Benefit? b = context.Set<Benefit>().SingleOrDefault(b => b.Id == pb.BenefitId);
                    if (!pass.Benefits.Contains(b.BenefitName))
                    {
                        this.context.Set<PassAndBenefit>().Remove(pb);
                        this.context.SaveChanges();
                    
                    }
                }
            }
            return GetPassById(id);
            

        }

        public PassResponse? DeleteOrMakePassInactive(int id)
        {
            Pass? passToDelete = GetPassModelById(id);
            if (passToDelete == null)
                return null;
            if (!passToDelete.IsActive)
                return null;
            if (this.purchasesRepository.GetAllPurchases().Any(purchase => purchase.PassId == id))
            {
                passToDelete.IsActive = false;
                passToDelete.IsHighlighted = false;
                this.context.Entry(passToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                this.context.SaveChanges();
                return new PassResponse(passToDelete);
            }
            this.context.Set<Pass>().Remove(passToDelete);
            this.context.SaveChanges();
            return new PassResponse(passToDelete);


        }
    }
}
