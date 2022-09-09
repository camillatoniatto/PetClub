using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.PurchaseOrder
{
    public class CreatePurchaseOrderViewModel
    {
        public string IdUser { get; set; }
        public string IdPet { get; set; }
        public string IdPaymentMethod { get; set; }
        public string FullName { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Observations { get; set; }
    }
}
