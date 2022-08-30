using PetClub.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Domain.Entities
{
    public class User : EntityBase
    {
        public string FullName { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public Boolean ChangePassword { get; set; } = false;
        public string Image { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPartner { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
