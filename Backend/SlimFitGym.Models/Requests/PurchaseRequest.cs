using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Requests
{
    public class PurchaseRequest
    {
        public int PassId { get; set; }
        public int AccountId { get; set; }
    }
}
