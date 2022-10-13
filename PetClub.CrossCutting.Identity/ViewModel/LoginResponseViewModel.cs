﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.CrossCutting.Identity.ViewModel
{
    public class LoginResponseViewModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public DateTime DateExpire { get; set; }
        public string IdUser { get; set; }
        public string FullName { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public string LastNumberCard { get; set; }
        public string NameCard { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public int UserPerfil { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPartner { get; set; }
        //public UserTokenViewModel User { get; set; }
    }
}
