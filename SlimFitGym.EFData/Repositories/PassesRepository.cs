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
        public PassesRepository(SlimFitGymContext context)
        {
            this.context = context;
        }

        public List<PassResponse> GetAllActivePasses()
        {
            List<PassResponse> passes = context.Set<Pass>().Where(p=>p.IsActive).Select(p=>new PassResponse(p)).ToList();

            foreach (PassResponse p in passes)
            {
                List<PassAndBenefit> pbs = context.Set<PassAndBenefit>().Where(pb=>pb.PassId==p.Id).ToList();
                foreach (PassAndBenefit pb in pbs)
                {
                    p.Benefits = context.Set<Benefit>().Where(b=>b.Id==pb.Id).Select(b=>b.BenefitName).ToList();              
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
                    p.Benefits = context.Set<Benefit>().Where(b => b.Id == pb.Id).Select(b => b.BenefitName).ToList();
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
                pass.Benefits = context.Set<Benefit>().Where(b => b.Id == pb.Id).Select(b => b.BenefitName).ToList();
            }

            return pass;
        }

        public PassResponse? GetPassById(int id)
        {
            PassResponse? pass = context.Set<Pass>().Where(p =>p.Id == id).Select(p => new PassResponse(p)).SingleOrDefault();
            if (pass == null)
                return null;

            List<PassAndBenefit> pbs = context.Set<PassAndBenefit>().Where(pb => pb.PassId == pass.Id).ToList();
            foreach (PassAndBenefit pb in pbs)
            {
                pass.Benefits = context.Set<Benefit>().Where(b => b.Id == pb.Id).Select(b => b.BenefitName).ToList();
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

            foreach (string bName in pass.Benefits)
            {
                if (bName!="")
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
                        PassAndBenefit? pb = this.context.Set<PassAndBenefit>().SingleOrDefault(pb=>pb.PassId==passToSave.Id && pb.BenefitId == b.Id);
                        if (pb==null)
                        {
                            PassAndBenefit newPb = this.context.Set<PassAndBenefit>().Add(new PassAndBenefit() { BenefitId = b.Id, PassId = savedPass.Id }).Entity;
                            this.context.SaveChanges();
      
                        }
                    }         
                }
            }


            return GetPassById(savedPass.Id);
        }

        public PassResponse? UpdatePass(int id, PassRequest pass)
        {
            if (pass.Id <= 0)
                throw new Exception("Ilyen edző nem létezik");
            if (id != pass.Id)
                throw new Exception("Érvénytelen azonosító.");
            if (pass == null)
                throw new Exception("Hibás kérés.");

            Pass? p = context.Set<Pass>().SingleOrDefault(p=>p.Id == id);
            if (p == null)
                throw new Exception("Nem található ilyen bérlet.");
            MakePassInactive(id);

            return NewPass(pass);
        }

        public PassResponse? MakePassInactive(int id)
        {
            Pass? passToDelete = this.context.Set<Pass>().SingleOrDefault(p => p.Id == id && p.IsActive);
            if (passToDelete == null)
                return null;

            passToDelete.IsActive = false;
            passToDelete.IsHighlighted = false;
            this.context.Entry(passToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return new PassResponse(passToDelete);
        }
    }
}
