using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.Scheduler
{
    public class CreateSchedulerViewModel
    {
        public string IdPartner { get; set; }
        public string IdPet { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public int ServiceType { get; set; }
        public int SchedulerSituation { get; set; }
    }
}
