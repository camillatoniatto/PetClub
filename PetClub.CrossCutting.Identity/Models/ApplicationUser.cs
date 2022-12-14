using Microsoft.AspNetCore.Identity;
using PetClub.Domain.Entities;
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
        public bool EmailConfirmed { get; set; } = false;
        public string PhoneNumber { get; set; }
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
        public bool AcceptedTermsOfUse { get; set; } = false;
        public bool IsActive { get; set; }
        public DateTime WriteDate { get; set; } = DateTime.Now.ToBrasilia();
    }
}
