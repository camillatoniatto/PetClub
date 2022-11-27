using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.Pet
{
    public class GetSchedulerPetViewModel
    {
        public GetSchedulerPetViewModel(string idScheduler, string idPartner, string partnerFullName, string idClient, string idPet, string petName, DateTime startDate, DateTime finalDate, string startDateString, string finalDateString, int serviceType, string serviceTypeString, int schedulerSituation, string schedulerSituationString, string writeDate)
        {
            IdScheduler = idScheduler;
            IdPartner = idPartner;
            PartnerFullName = partnerFullName;
            IdClient = idClient;
            IdPet = idPet;
            PetName = petName;
            StartDate = startDate;
            FinalDate = finalDate;
            StartDateString = startDateString;
            FinalDateString = finalDateString;
            ServiceType = serviceType;
            ServiceTypeString = serviceTypeString;
            SchedulerSituation = schedulerSituation;
            SchedulerSituationString = schedulerSituationString;
            WriteDate = writeDate;
        }

        public string IdScheduler { get; set; }
        public string IdPartner { get; set; }
        public string PartnerFullName { get; set; }
        public string IdClient { get; set; }
        public string IdPet { get; set; }
        public string PetName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public string StartDateString { get; set; }
        public string FinalDateString { get; set; }
        public int ServiceType { get; set; }
        public string ServiceTypeString { get; set; }
        public int SchedulerSituation { get; set; }
        public string SchedulerSituationString { get; set; }
        public string WriteDate { get; set; }
    }
}
