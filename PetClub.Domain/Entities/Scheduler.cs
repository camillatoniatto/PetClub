using PetClub.Domain.Entities.Base;
using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Domain.Entities
{
    public class Scheduler : EntityBase
    {
        public Scheduler(string idPartner, string idPet, DateTime startDate, DateTime finalDate, ServiceType serviceType, SchedulerSituation schedulerSituation, DateTime writeDate)
        {
            IdPartner = idPartner;
            IdPet = idPet;
            StartDate = startDate;
            FinalDate = finalDate;
            ServiceType = serviceType;
            SchedulerSituation = schedulerSituation;
            WriteDate = writeDate;
        }

        public string IdPartner { get; set; }
        public string IdPet { get; set; }
        public Pet Pet { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public ServiceType ServiceType { get; set; }
        public SchedulerSituation SchedulerSituation { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
