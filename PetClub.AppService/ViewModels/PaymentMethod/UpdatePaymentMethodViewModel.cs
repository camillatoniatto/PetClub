using PetClub.Domain.Enum;

namespace PetClub.AppService.ViewModels.PaymentMethod
{
    public class UpdatePaymentMethodViewModel
    {
        public string IdPaymentMethod { get; set; }
        public string Title { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool IsInstallment { get; set; }
        public int NumberInstallments { get; set; }
        public decimal AdminTax { get; set; }
    }
}
