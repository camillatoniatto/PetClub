using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace PetClub.AppService.ViewModels.CashFlow
{
    public class GetCashFlowViewModel
    {
        public GetCashFlowViewModel(string status, string title, string description, string idUserCreate, string userCreateName, string? idPurchaseOrder, string? idPaymentMethod, string? paymentMethod, decimal launchValue, decimal netValue, string expirationDate, string writeOffDate, string idUserWriteOff, string userWriteOffName, string idUserInactivate, string userInactivateName, bool isOutflow, DateTime writeDate)
        {
            Status = status;
            Title = title;
            Description = description;
            IdUserCreate = idUserCreate;
            UserCreateName = userCreateName;
            IdPurchaseOrder = idPurchaseOrder;
            IdPaymentMethod = idPaymentMethod;
            PaymentMethod = paymentMethod;
            LaunchValue = launchValue;
            NetValue = netValue;
            ExpirationDate = expirationDate;
            WriteOffDate = writeOffDate;
            IdUserWriteOff = idUserWriteOff;
            UserWriteOffName = userWriteOffName;
            IdUserInactivate = idUserInactivate;
            UserInactivateName = userInactivateName;
            IsOutflow = isOutflow;
            WriteDate = writeDate;
        }

        public string Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IdUserCreate { get; set; }
        public string UserCreateName { get; set; }
        public string? IdPurchaseOrder { get; set; }
        public string? IdPaymentMethod { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal LaunchValue { get; set; }
        public decimal NetValue { get; set; }
        public string ExpirationDate { get; set; }
        public string WriteOffDate { get; set; }
        public string IdUserWriteOff { get; set; }
        public string UserWriteOffName { get; set; }
        public string IdUserInactivate { get; set; }
        public string UserInactivateName { get; set; }
        public bool IsOutflow { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
