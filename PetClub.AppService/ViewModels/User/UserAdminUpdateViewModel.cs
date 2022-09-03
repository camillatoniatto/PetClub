using PetClub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.User
{
    public class UserAdminUpdateViewModel
    {
        public string Id { get; set; }
        // roles
        public bool IsAdmin { get; set; }
        public bool IsPartner { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
