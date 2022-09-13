using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace PetClub.AppService.ViewModels.CashFlow
{
    public class CreateCashFlowViewModel
    {
        public CreateCashFlowViewModel(string title, string description, string idUserCreate, string idPurchaseOrder, string idPaymentMethod, decimal launchValue, DateTime expirationDate, DateTime writeOffDate, bool isOutflow)
        {
            Title = title;
            Description = description;
            IdUserCreate = idUserCreate;
            IdPurchaseOrder = idPurchaseOrder;
            IdPaymentMethod = idPaymentMethod;
            LaunchValue = launchValue;
            ExpirationDate = expirationDate;
            WriteOffDate = writeOffDate;
            this.isOutflow = isOutflow;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string IdUserCreate { get; set; }
        public string IdPurchaseOrder { get; set; }
        public string IdPaymentMethod { get; set; }
        public decimal LaunchValue { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime WriteOffDate { get; set; }
        public bool isOutflow { get; set; }
    }
}
