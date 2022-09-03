using PetClub.Domain.Entities.Base;
using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Domain.Entities
{
    public class PaymentMethod : EntityBase
    {
        public string Title { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool IsInstallment { get; set; }
        public int NumberInstallments { get; set; }
        public decimal AdminTax { get; set; }
    }
}
