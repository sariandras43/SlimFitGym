using Microsoft.EntityFrameworkCore.Internal;
using SlimFitGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Machine = SlimFitGym.Models.Machine;

namespace SlimFitGym.EFData.Repositories
{
    public class MachinesRepository
    {
        readonly SlimFitGymContext context;

        public MachinesRepository(SlimFitGymContext context)
        {
            this.context = context;
        }

        public List<Machine> GetAllMachine()
        {
            return context.Set<Machine>().ToList();
        }

        public Machine? GetMachineById(int id)
        {
            var result = context.Set<Machine>().SingleOrDefault(m => m.Id == id);
            if (result!=null)
                return result;

            throw new Exception("Nincs ilyen gép");
        }

        public Machine? NewMachine(Machine newMachine)
        {

            if (newMachine == null)
                throw new Exception("Hibás kérés.");
            if (newMachine.Name == null || newMachine.Name.Length == 0)
                throw new Exception("Kötelező kitölteni a név mezőt.");
            if (newMachine.Name.Length > 100)
                throw new Exception("A név maximum 100 karakter hosszú lehet");
            if (newMachine.Description!=null && newMachine.Description!.Length > 500)
                throw new Exception("A leírás maximum 500 karakter hosszú lehet");
            if (context.Set<Machine>().Any(m => m.Name == newMachine.Name))
                throw new Exception("Ilyen gép már létezik.");

            Machine savedMachine = this.context.Set<Machine>().Add(newMachine).Entity;

            this.context.SaveChanges();
            return savedMachine;
        }

        public Machine? UpdateMachine (int id, Machine machine)
        {
            if (id != machine.Id)
                throw new Exception("Érvénytelen azonosító.");
            if (!this.context.Set<Machine>().Any(m => m.Id == id))
                throw new Exception("Nem létezik a gép.");
            if (machine == null)
                throw new Exception("Hibás kérés.");
            if (machine.Name == null || machine.Name.Length == 0)
                throw new Exception("Kötelező kitölteni a név mezőt.");
            if (machine.Name.Length > 100)
                throw new Exception("A név maximum 100 karakter hosszú lehet");
            if (machine.Description != null && machine.Description!.Length > 500)
                throw new Exception("A leírás maximum 500 karakter hosszú lehet");
            if (context.Set<Machine>().Any(m => m.Name == machine.Name))
                throw new Exception("Ilyen gép már létezik.");


            this.context.Entry(machine).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return machine;
        }

        public Machine? DeleteMachine(int id)
        {
            var machineToDelete = this.context.Set<Machine>().SingleOrDefault(m => m.Id == id);
            if (machineToDelete == null)
                throw new Exception("Nem létezik a gép.");

            this.context.Set<Machine>().Remove(machineToDelete);
            this.context.SaveChanges();
            return machineToDelete;
        }



    }
}
