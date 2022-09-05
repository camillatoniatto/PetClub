using Microsoft.AspNetCore.Identity;
using PetClub.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.CrossCutting.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }

        public string FullName { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public string Image { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPartner { get; set; }
        public bool IsActive { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
