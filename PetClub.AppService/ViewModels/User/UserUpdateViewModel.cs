using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.User
{
    public class UserUpdateViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public DateTime Birthdate { get; set; }
        public string PhoneNumber { get; set; }
    }
}
