using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.PurchaseOrderItem
{
    public class UpdatePurchaseOrderItemViewModel
    {
        public string IdPurchaseOrderItem { get; set; }
        public string IdService { get; set; }
        public int Quantity { get; set; }
        public bool Delete { get; set; }
    }
}
