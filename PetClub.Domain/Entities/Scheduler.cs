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
        public string IdUsersPartners { get; set; }
        public string IdPet { get; set; }
        public Pet Pet { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public ServiceType ServiceType { get; set; }
        public SchedulerSituation SchedulerSituation { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
