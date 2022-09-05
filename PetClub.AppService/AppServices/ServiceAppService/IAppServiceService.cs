using PetClub.AppService.ViewModels.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.ServiceAppService
{
    public interface IAppServiceService
    {
        Task<string> CreateService(CreateServiceViewModel model, string idUser);
        Task DeleteService(string idService);
        Task<GetServiceViewModel> GetServiceById(string idService);
        Task<List<GetServiceViewModel>> GetServiceUser(string idUser);
        Task UpdateService(UpdateServiceViewModel model);
    }
}
