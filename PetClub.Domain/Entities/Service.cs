using PetClub.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.Domain.Enum;

namespace PetClub.Domain.Entities
{
    public class Service : EntityBase
    {
        public Service(string idPartner, string title, string description, ServiceType serviceType, bool singleUser, DateTime dateDuration, decimal value, DateTime writeDate)
        {
            IdPartner = idPartner;
            Title = title;
            Description = description;
            ServiceType = serviceType;
            SingleUser = singleUser;
            DateDuration = dateDuration;
            Value = value;
            WriteDate = writeDate;
        }

        public string IdPartner { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ServiceType ServiceType { get; set; }
        public bool SingleUser { get; set; }
        public DateTime DateDuration { get; set; }
        public decimal Value { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
