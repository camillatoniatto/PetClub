using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.Scheduler
{
    public class UpdateSchedulerViewModel
    {
        public string IdScheduler { get; set; }
        public string IdPartner { get; set; }
        public string IdPet { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public ServiceType ServiceType { get; set; }
        public SchedulerSituation SchedulerSituation { get; set; }
    }
}
