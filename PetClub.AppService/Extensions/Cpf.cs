using PetClub.Domain.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.Extensions
{
    public class Cpf : ValidationAttribute
    {
        public Cpf() { }

        public override bool IsValid(object value)
        {
            return CpfCnpjValidate.Validar(value.ToString());
        }
    }
}
