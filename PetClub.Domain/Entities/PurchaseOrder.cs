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
        public PurchaseOrder(string idPartner, string idUser, string idPet, string idPaymentMethod, string fullName, string cpf, string email, PurchaseOrderSituation purchaseOrderSituation, PaymentSituation paymentSituation, string observations, DateTime writeDate)
        {
            IdPartner = idPartner;
            IdUser = idUser;
            IdPet = idPet;
            IdPaymentMethod = idPaymentMethod;
            FullName = fullName;
            Cpf = cpf;
            Email = email;
            PurchaseOrderSituation = purchaseOrderSituation;
            PaymentSituation = paymentSituation;
            Observations = observations;
            WriteDate = writeDate;
        }

        public string IdPartner { get; set; }
        public User User { get; set; }
        public string IdUser { get; set; }
        public string IdPet { get; set; }
        public string IdPaymentMethod { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string FullName { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public IEnumerable<PurchaseOrderItem> PurchaseOrderItem { get; set; }
        public PurchaseOrderSituation PurchaseOrderSituation { get; set; }
        public PaymentSituation PaymentSituation { get; set; }
        public string Observations { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
