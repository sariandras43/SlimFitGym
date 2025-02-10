using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using SlimFitGym.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
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

        public AccountResponse? Register(RegistrationRequest registration) 
        {
            if (registration.Name.Length > 100)
                throw new Exception("Túl hosszú név.");
            if(registration.Password.Length<8)
                throw new Exception("Túl rövid jelszó.");
            Regex emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
            Match email = emailRegex.Match(registration.Email);
            if (!email.Success)
                throw new Exception("Érvénytelen formátumú email cím.");

            bool isMadeUpOfDigits = true;
            for (int i = 1; i < registration.Phone.Length; i++)
            {
                if (!char.IsDigit(registration.Phone[i]))
                    isMadeUpOfDigits = false;
            }
            if (!registration.Phone.StartsWith('+') || registration.Phone.Length>16 || !isMadeUpOfDigits || registration.Phone.Length < 7)
                throw new Exception("Érvénytelen formátumú telefonszám.");
            Account? account = context.Set<Account>().SingleOrDefault(a=>a.Email==registration.Email);
            if (account != null)
                throw new Exception("Ezzel az email címmel már regisztráltak");

            //For developing only, in production hasing+salting will be used!!
            //For developing only, in production hasing+salting will be used!!
            //For developing only, in production hasing+salting will be used!!
            //For developing only, in production hasing+salting will be used!!
            //For developing only, in production hasing+salting will be used!!

            Account newAccount = new Account() { Email=registration.Email,isActive=true,Name=registration.Name
                ,Password=registration.Password,Phone= registration.Phone.Replace(" ", ""),
                Role="user"};

            Account savedAccount = this.context.Set<Account>().Add(newAccount).Entity;
            this.context.SaveChanges();
            return new AccountResponse(savedAccount);

        }

        public AccountResponse? ModifyAccount(int id, ModifyAccountRequest request)
        {
            if (request.Id!= id)
                throw new Exception("Érvénytelen azonosító.");
            if (request == null)
                throw new Exception("Érvénytelen kérés.");
            if (request.Name.Length > 100)
                throw new Exception("Túl hosszú név.");
            if (request.Password.Length < 8)
                throw new Exception("Túl rövid jelszó.");
            Regex emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
            Match email = emailRegex.Match(request.Email);
            if (!email.Success)
                throw new Exception("Érvénytelen formátumú email cím.");
            bool isMadeUpOfDigits = true;
            for (int i = 1; i < request.Phone.Length; i++)
            {
                if (!char.IsDigit(request.Phone[i]))
                    isMadeUpOfDigits = false;
            }
            if (!request.Phone.StartsWith('+') || request.Phone.Length > 16 || !isMadeUpOfDigits || request.Phone.Length < 7)
                throw new Exception("Érvénytelen formátumú telefonszám.");
            Account? account = context.Set<Account>().SingleOrDefault(a => a.Email == request.Email);
            if (account == null)
                return null;
            if (account.Id != request.Id)
                throw new Exception("Ezzel az email címmel már van felhasználói fiók.");

            account.Email=request.Email;
            account.Phone=request.Phone;
            account.Password=request.Password;
            account.Name = request.Name;
            account.Role = request.Role;

            this.context.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.context.SaveChanges();
            return new AccountResponse(account);
        }

        public AccountResponse? DeleteAccount(int id)
        {
            throw new NotImplementedException();
        }
    }
}
