using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.PaymentMethod
{
    public class GetPaymentMethodViewModel
    {
        public GetPaymentMethodViewModel(string idPaymentMethod, string title, string paymentType, bool isInstallment, int numberInstallments, decimal adminTax)
        {
            IdPaymentMethod = idPaymentMethod;
            Title = title;
            PaymentType = paymentType;
            IsInstallment = isInstallment;
            NumberInstallments = numberInstallments;
            AdminTax = adminTax;
        }

        public string IdPaymentMethod { get; set; }
        public string Title { get; set; }
        public string PaymentType { get; set; }
        public bool IsInstallment { get; set; }
        public int NumberInstallments { get; set; }
        public decimal AdminTax { get; set; }
    }
}
