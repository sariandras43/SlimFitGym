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
    public class AccountRepository
    {
        readonly SlimFitGymContext context;

        public AccountRepository(SlimFitGymContext context)
        {
            this.context = context;
        }

        public AccountResponse? Login(LoginRequest login) 
        {
            Account? a= context.Set<Account>().SingleOrDefault(a=>a.Email==login.Email);
            if (a == null)
                return null;
            //For developing only, in production hasing+salting will be used!!
            if (login.Password != a.Password)
                throw new Exception("Helytelen email cím vagy jelszó.");
            return new AccountResponse(a);
            
        }
    }
}
