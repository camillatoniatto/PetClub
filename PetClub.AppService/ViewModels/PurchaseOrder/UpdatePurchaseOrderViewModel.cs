using PetClub.AppService.ViewModels.PurchaseOrderItem;
using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.PurchaseOrder
{
    public class UpdatePurchaseOrderViewModel
    {
        public string IdPurchaseOrder { get; set; }
        //public string IdPartner { get; set; }
        //public string IdPet { get; set; }
        public string IdPaymentMethod { get; set; }
        //public string PaymentType { get; set; }
        //public string IdUser { get; set; }
        //public string FullName { get; set; }
        //public string Cpf { get; set; }
        //public string Email { get; set; }
        public PurchaseOrderSituation PurchaseOrderSituation { get; set; }
        public PaymentSituation PaymentSituation { get; set; }
        public string Observations { get; set; }
        public List<UpdatePurchaseOrderItemViewModel> PurchaseOrderItem { get; set; }
        //public string WriteDate { get; set; }
        //public string CreateDate { get; set; }
    }
}
