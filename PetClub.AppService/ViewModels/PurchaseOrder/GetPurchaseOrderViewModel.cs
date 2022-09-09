using PetClub.AppService.ViewModels.PurchaseOrderItem;
using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.PurchaseOrder
{
    public class GetPurchaseOrderViewModel
    {
        public GetPurchaseOrderViewModel(string idPurchaseOrder, string idPartner, string partnerName, string idPet, string petName, string idPaymentMethod, string paymentType, string idUser, string fullName, string cpf, string email, string purchaseOrderSituation, string paymentSituation, string observations, List<GetOrderItensViewModel> purchaseOrderItem, string writeDate, string createDate)
        {
            IdPurchaseOrder = idPurchaseOrder;
            IdPartner = idPartner;
            PartnerName = partnerName;
            IdPet = idPet;
            PetName = petName;
            IdPaymentMethod = idPaymentMethod;
            PaymentType = paymentType;
            IdUser = idUser;
            FullName = fullName;
            Cpf = cpf;
            Email = email;
            PurchaseOrderSituation = purchaseOrderSituation;
            PaymentSituation = paymentSituation;
            Observations = observations;
            PurchaseOrderItem = purchaseOrderItem;
            WriteDate = writeDate;
            CreateDate = createDate;
        }

        public string IdPurchaseOrder { get; set; }
        public string IdPartner { get; set; }
        public string PartnerName { get; set; }
        public string IdPet { get; set; }
        public string PetName { get; set; }
        public string IdPaymentMethod { get; set; }
        public string PaymentType { get; set; }
        public string IdUser { get; set; }
        public string FullName { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string PurchaseOrderSituation { get; set; }
        public string PaymentSituation { get; set; }
        public string Observations { get; set; }
        public List<GetOrderItensViewModel> PurchaseOrderItem { get; set; }
        public string WriteDate { get; set; }
        public string CreateDate { get; set; }
    }
}
