using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.User
{
    public class GetUsersAmountViewModel
    {
        public GetUsersAmountViewModel(int admin, int partner, int user)
        {
            Admin = admin;
            Partner = partner;
            User = user;
        }

        public int Admin { get; set; }
        public int Partner { get; set; }
        public int User { get; set; }
    }
}
