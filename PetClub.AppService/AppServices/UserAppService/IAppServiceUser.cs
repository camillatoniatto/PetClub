using PetClub.AppService.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.UserAppService
{
    public interface IAppServiceUser
    {
        Task<UserBasicViewModel> GetByCpf(string Cpf);
    }
}
