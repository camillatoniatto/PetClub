using PetClub.AppService.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;
using Xunit.Sdk;

namespace PetClub.AppService.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Usuário")]
        [Required(ErrorMessage = "O usuário é obrigatório.")]
        [Cpf(ErrorMessage = "O Cpf é inválido")]
        public string UserName { get; set; }
        [Display(Name = "Senha")]
        [MaxLength(32, ErrorMessage = "")]
        public string Password { get; set; }

        public string RefreshToken { get; set; }
        public string GrantType { get; set; }
    }
}
