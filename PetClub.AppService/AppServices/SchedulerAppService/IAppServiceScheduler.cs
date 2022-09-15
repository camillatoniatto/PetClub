using PetClub.AppService.ViewModels.Pet;
using PetClub.AppService.ViewModels.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.SchedulerAppService
{
    public interface IAppServiceScheduler
    {
        Task CheckAvailable(string idPet, DateTime startDate, DateTime endDate);
        Task<string> CreateScheduler(CreateSchedulerViewModel model);
        Task DeleteScheduler(string idScheduler);
        Task<GetSchedulerViewModel> GetSchedulersById(string idScheduler);
        Task<List<GetSchedulerViewModel>> GetSchedulersByPartner(string idPartner);
        Task<List<GetSchedulerPetViewModel>> GetSchedulersByPet(string idPet);
        Task UpdateScheduler(UpdateSchedulerViewModel model);
    }
}
