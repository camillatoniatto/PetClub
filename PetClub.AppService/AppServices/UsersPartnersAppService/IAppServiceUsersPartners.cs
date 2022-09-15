using PetClub.AppService.ViewModels.UsersPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.UsersPartnersAppService
{
    public interface IAppServiceUsersPartners
    {
        Task<string> CreateUsersPartners(string idUser, string idPartner);
        Task<GetUsersPartnersViewModel> GetByIdUsersPartners(string idUsersPartners);
        Task<List<GetUsersPartnersViewModel>> GetUsersPartners(string idPartner);
    }
}
