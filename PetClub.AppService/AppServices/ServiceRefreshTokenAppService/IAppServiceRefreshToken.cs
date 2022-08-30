using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.AppService.ViewModels.User;
using PetClub.AppService.ViewModels.Account;

namespace PetClub.AppService.AppServices.ServiceRefreshTokenAppService
{
    public interface IAppServiceRefreshToken
    {
        Task Create(RefreshTokenDataViewModel RefreshTokenDataViewModel);
        Task<string> ValidateToken(LoginViewModel loginViewModel, string idUser);
    }
}
