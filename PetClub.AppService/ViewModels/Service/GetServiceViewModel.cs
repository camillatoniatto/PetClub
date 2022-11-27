using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.Service
{
    public class GetServiceViewModel
    {
        public GetServiceViewModel(string idService, string idPartner, string title, string description, string serviceTypeString, int serviceType, bool singleUser, string dateDuration, string valueString, decimal value, string writeDate, string partnerFullName, int sold)
        {
            IdService = idService;
            IdPartner = idPartner;
            Title = title;
            Description = description;
            ServiceTypeString = serviceTypeString;
            ServiceType = serviceType;
            SingleUser = singleUser;
            DateDuration = dateDuration;
            ValueString = valueString;
            Value = value;
            WriteDate = writeDate;
            PartnerFullName = partnerFullName;
            Sold = sold;
        }

        public string IdService { get; set; }
        public string IdPartner { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ServiceTypeString { get; set; }
        public int ServiceType { get; set; }
        public bool SingleUser { get; set; }
        public string DateDuration { get; set; }
        public string ValueString { get; set; }
        public decimal Value { get; set; }
        public string WriteDate { get; set; }
        public string PartnerFullName { get; set; }
        public int Sold { get; set; }
    }
}
