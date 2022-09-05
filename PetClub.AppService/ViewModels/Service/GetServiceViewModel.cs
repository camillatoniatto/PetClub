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
        public GetServiceViewModel(string idService, string idPartner, string title, string description, string serviceType, bool singleUser, string dateDuration, string value, string writeDate)
        {
            IdService = idService;
            IdPartner = idPartner;
            Title = title;
            Description = description;
            ServiceType = serviceType;
            SingleUser = singleUser;
            DateDuration = dateDuration;
            Value = value;
            WriteDate = writeDate;
        }

        public string IdService { get; set; }
        public string IdPartner { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ServiceType { get; set; }
        public bool SingleUser { get; set; }
        public string DateDuration { get; set; }
        public string Value { get; set; }
        public string WriteDate { get; set; }
    }
}
