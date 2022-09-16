using Microsoft.AspNetCore.Identity;
using PetClub.Domain.Entities;
using PetClub.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PetClub.CrossCutting.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }

        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string FullName { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        //public string TwoFactorEnabled { get; set; }
        //public DateTimeOffset LockoutEnd { get; set; }
        //public bool LockoutEnabled { get; set; }
        //public int AccessFailedCount { get; set; }
        public DateTime Birthdate { get; set; }
        public Boolean ChangePassword { get; set; } = false;
        public string Image { get; set; }
        // roles
        public bool IsAdmin { get; set; }
        public bool IsPartner { get; set; }

        // endereço
        public string AddressName { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        //public virtual IEnumerable<Pet> Pet { get; set; }
        //public virtual IEnumerable<UsersPartners> UsersPartners { get; set; }
        public bool AcceptedTermsOfUse { get; set; }
        public bool IsActive { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
