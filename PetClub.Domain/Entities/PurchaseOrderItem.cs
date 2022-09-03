using PetClub.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Domain.Entities
{
    public class PurchaseOrderItem : EntityBase
    {
        public string IdPurchaseOrder { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public string IdService { get; set; }
        public Service Service { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
