using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.Account
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string Cpf { get; set; }
        public bool HasRegistered { get; set; }
    }
}
