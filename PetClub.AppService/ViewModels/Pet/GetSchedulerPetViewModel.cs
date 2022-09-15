using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.Pet
{
    public class GetSchedulerPetViewModel
    {
        public GetSchedulerPetViewModel(string idScheduler, string idPartner, string partnerFullName, GetPetViewModel detailPet, string startDate, string finalDate, string serviceType, string schedulerSituation, string writeDate)
        {
            IdScheduler = idScheduler;
            IdPartner = idPartner;
            PartnerFullName = partnerFullName;
            DetailPet = detailPet;
            StartDate = startDate;
            FinalDate = finalDate;
            ServiceType = serviceType;
            SchedulerSituation = schedulerSituation;
            WriteDate = writeDate;
        }

        public string IdScheduler { get; set; }
        public string IdPartner { get; set; }
        public string PartnerFullName { get; set; }
        public GetPetViewModel DetailPet { get; set; }
        public string StartDate { get; set; }
        public string FinalDate { get; set; }
        public string ServiceType { get; set; }
        public string SchedulerSituation { get; set; }
        public string WriteDate { get; set; }
    }
}
