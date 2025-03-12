using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SlimFitGym.EFData.Interfaces;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Machine = SlimFitGym.Models.Models.Machine;

namespace SlimFitGym.EFData.Repositories
{
    public class MachinesRepository:IMachinesRepository
    {
        readonly SlimFitGymContext context;
        readonly IImagesRepository imagesRepository;

        public MachinesRepository(SlimFitGymContext context, IImagesRepository imagesRepository)
        {
            this.context = context;
            this.imagesRepository = imagesRepository;   
        }

        public List<MachineResponse> GetAllMachine()
        {
            return context.Set<Machine>().Select(m=>new MachineResponse(m, imagesRepository.GetImageUrlsByMachineId(m.Id))).ToList();
        }

        public MachineResponse? GetMachineById(int id)
        {
            Machine? result = context.Set<Machine>().SingleOrDefault(m => m.Id == id);
            if (result!=null)
                return new MachineResponse(result,imagesRepository.GetImageUrlsByMachineId(id));

            return null;
        }

        public MachineResponse? NewMachine(MachineRequest newMachine)
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

            Machine machineToAdd = new Machine()
            {
                Name = newMachine.Name,
                Description = newMachine.Description,
            };
            Machine savedMachine = this.context.Set<Machine>().Add(machineToAdd).Entity;

            this.context.SaveChanges();

            List<Image> images= imagesRepository.UploadImagesToMachine(newMachine.Images,savedMachine.Id);

            return new MachineResponse(savedMachine,imagesRepository.GetImageUrlsByMachineId(savedMachine.Id));
        }

        public MachineResponse? UpdateMachine (int id, MachineRequest machine)
        {
            if (id != machine.Id)
                throw new Exception("Érvénytelen azonosító.");
            Machine? m = context.Set<Machine>().FirstOrDefault(m => m.Id == id);
            if (m==null)
                return null;
            if (machine == null)
                throw new Exception("Hibás kérés.");
            if (machine.Description != null && machine.Description!.Length > 500)
                throw new Exception("A leírás maximum 500 karakter hosszú lehet");
            if (machine.Name == null || machine.Name.Length == 0)
                throw new Exception("Kötelező kitölteni a név mezőt.");
            if (machine.Name.Length > 100)
                throw new Exception("A név maximum 100 karakter hosszú lehet");
            if (m.Name!=machine.Name && context.Set<Machine>().Any(m => m.Name == machine.Name))
                throw new Exception("Ilyen nevű gép már létezik.");
            m.Name = machine.Name;
           
            m.Description = machine.Description;


            this.context.Entry(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            //imagesRepository.DeleteImagesByMachineId(id);
            imagesRepository.UploadImagesToMachine(machine.Images, id);
            return new MachineResponse(m,imagesRepository.GetImageUrlsByMachineId(id));
        }

        public Machine? DeleteMachine(int id)
        {
            var machineToDelete = this.context.Set<Machine>().SingleOrDefault(m => m.Id == id);
            if (machineToDelete == null)
                return null;

            var machineRoomConnections = this.context.Set<RoomAndMachine>().Where(rm=>rm.MachineId == id);
            foreach (var connection in machineRoomConnections)
            {
                this.context.Set<RoomAndMachine>().Remove(connection);
            }
            imagesRepository.DeleteImagesByMachineId(id);
            this.context.Set<Machine>().Remove(machineToDelete);
            this.context.SaveChanges();
            return machineToDelete;
        }



    }
}
