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
    public class PassesRepository
    {

        readonly SlimFitGymContext context;
        readonly PurchasesRepository purchasesRepository;
        public PassesRepository(SlimFitGymContext context,PurchasesRepository purchasesRepository)
        {
            this.context = context;
            this.purchasesRepository = purchasesRepository;
        }

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

        public PassResponse? NewPass(PassRequest pass)
        {
            if (pass.Price < 0)
                throw new Exception("Érvénytelen ár.");
            if (pass.Name.Length > 100)
                throw new Exception("Túl hosszú név.");
            if (pass.MaxEntries < 1 && pass.Days<1)
                throw new Exception("Kötelező megadni legalább a maximum belépések számát vagy a felhasználható napok értékét.");


            Pass passToSave = new Pass()
            {
                Days = pass.Days,
                IsActive = pass.isActive,
                IsHighlighted = pass.isHighlighted,
                MaxEntries = pass.MaxEntries,
                Name = pass.Name,
                Price = pass.Price
                
            };
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

        public dynamic? UpdatePass(int id, PassRequest pass)
        {

            //return context.Set<PassAndBenefit>().ToList();
            if (pass.Id <= 0)
                throw new Exception("Ilyen bérlet nem létezik");
            if (id != pass.Id)
                throw new Exception("Érvénytelen azonosító.");
            if (pass == null)
                throw new Exception("Hibás kérés.");

            Pass? p = context.Set<Pass>().SingleOrDefault(p=>p.Id == id && p.IsActive);
            if (p == null)
                throw new Exception("Nem található ilyen bérlet.");
            if (this.purchasesRepository.GetAllPurchases().Any(purchase=>purchase.PassId==id))
            {
                DeleteOrMakePassInactive(id);
                return NewPass(pass);
            }
            if (pass.Price < 0)
                throw new Exception("Érvénytelen ár.");
            if (pass.Name.Length > 100)
                throw new Exception("Túl hosszú név.");
            if (pass.MaxEntries < 1 && pass.Days < 1)
                throw new Exception("Kötelező megadni legalább a maximum belépések számát vagy a felhasználható napok értékét.");

            this.context.Entry(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();

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
