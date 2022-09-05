using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.PaymentMethod
{
    public class SelectPaymentMethodViewModel
    {
        public SelectPaymentMethodViewModel(string id, string value)
        {
            Id = id;
            Value = value;
        }

        public string Id { get; set; }
        public string Value { get; set; }
    }
}
