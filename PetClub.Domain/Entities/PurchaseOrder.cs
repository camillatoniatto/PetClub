using PetClub.Domain.Entities.Base;
using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Domain.Entities
{
    public class PurchaseOrder : EntityBase
    {
        public string IdUsersPartners { get; set; }
        public UsersPartners UsersPartners { get; set; }
        public string IdPaymentMethod { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string FullName { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public PurchaseOrderSituation PurchaseOrderSituation { get; set; }
        public PaymentSituation PaymentSituation { get; set; }
        public string Observations { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
