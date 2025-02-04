using SlimFitGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.EFData.Repositories
{
    public class MachinesRepository
    {
        readonly SlimFitGymContext context;

        public MachinesRepository(SlimFitGymContext context)
        {
            this.context = context;
        }

        public List<Machine>? GetAllMenuItems()
        {
            return this.context.Set<Machine>().ToList();
        }
    }
}
