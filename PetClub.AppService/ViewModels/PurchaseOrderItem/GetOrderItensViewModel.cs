using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.PurchaseOrderItem
{
    public class GetOrderItensViewModel
    {
        public GetOrderItensViewModel(string idPurchaseOrderItem, string idPurchaseOrder, string idService, string title, string description, int quantity, decimal value, string writeDate)
        {
            IdPurchaseOrderItem = idPurchaseOrderItem;
            IdPurchaseOrder = idPurchaseOrder;
            IdService = idService;
            Title = title;
            Description = description;
            Quantity = quantity;
            Value = value;
            WriteDate = writeDate;
        }

        public string IdPurchaseOrderItem { get; set; }
        public string IdPurchaseOrder { get; set; }
        public string IdService { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public string WriteDate { get; set; }
    }
}
