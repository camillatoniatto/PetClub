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
        public ApplicationUser(DateTime dateCreation, string fullName, string cpf, DateTime birthdate, bool changePassword, string image, bool isAdmin, bool isPartner)
        {
            DateCreation = dateCreation;
            FullName = fullName;
            Cpf = cpf;
            Birthdate = birthdate;
            ChangePassword = changePassword;
            Image = image;
            IsAdmin = isAdmin;
            IsPartner = isPartner;
        }

        public DateTime DateCreation { get; private set; }
        public string FullName { get; set; }
        public string Cpf { get; set; }
        public DateTime Birthdate { get; set; }
        public Boolean ChangePassword { get; set; } = false;
        public string Image { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPartner { get; set; }
    }
}
