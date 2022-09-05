using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.PaymentMethod
{
    public class CreatePaymentMethodViewModel
    {
        public CreatePaymentMethodViewModel(string title, PaymentType paymentType, bool isInstallment, int numberInstallments, decimal adminTax)
        {
            Title = title;
            PaymentType = paymentType;
            IsInstallment = isInstallment;
            NumberInstallments = numberInstallments;
            AdminTax = adminTax;
        }

        public string Title { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool IsInstallment { get; set; }
        public int NumberInstallments { get; set; }
        public decimal AdminTax { get; set; }
    }
}
