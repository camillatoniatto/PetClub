using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace PetClub.AppService.ViewModels.User
{
    public class UpdatePerfilUserViewModel
    {
        public string Id { get; set; }
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Name { get; set; }
        [RegularExpression(@"^(?('')(''.+?(?<!\\)''@)|(([0-9a_-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a_-z])@))+(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", ErrorMessage = "O email informado é inválido")]
        [Required(ErrorMessage = "O campo E-mail é obrigatório")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime Birthdate { get; set; } 
        //[ValidImageFile(ErrorMessage = "A extensão do arquivo não é compatível com imagem")]
        public IFormFile Image { get; set; }
        public string PhoneNumber { get; set; }
        // endereço
        public string AddressName { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
