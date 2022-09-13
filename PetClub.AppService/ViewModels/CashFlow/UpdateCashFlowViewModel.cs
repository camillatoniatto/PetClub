using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.CashFlow
{
    public class UpdateCashFlowViewModel
    {
        public string IdCashFlow { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IdUserCreate { get; set; }
        public string IdPurchaseOrder { get; set; }
        public string IdPaymentMethod { get; set; }
        public decimal LaunchValue { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime WriteOffDate { get; set; }
    }
}
