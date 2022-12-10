using PetClub.AppService.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit.Sdk;

namespace PetClub.AppService.ViewModels.Account
{
    public class UserViewModel
    {
        [Display(Name = "Nome completo")]
        [Required(ErrorMessage = "O campo nome completo é obrigatório")]
        public string FullName { get; set; }
        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O campo CPF é obrigatório")]
        [Cpf(ErrorMessage = "O CPF informado é invalido")]
        public string Cpf { get; set; }
        [RegularExpression(@"^(?('')(''.+?(?<!\\)''@)|(([0-9a_-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a_-z])@))+(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", ErrorMessage = "O email informado é inválido")]
        [Required(ErrorMessage = "O campo E-mail é obrigatório")]
        public string Email { get; set; }
        [Display(Name = "Telefone")]
        [RegularExpression(@"(^(?:(?:\+|00)?(55)\s?)?(?:\(?([1-9][0-9])?\)?\s?)(?:((?:9\d|[2-9])\d{3})\-?(\d{4})))", ErrorMessage = "O telefone informado é inválido")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório")]
        public DateTime Birthdate { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        // endereço
        public string AddressName { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool IsPartner { get; set; }
        public bool IsAdmin { get; set; }
    }
}
