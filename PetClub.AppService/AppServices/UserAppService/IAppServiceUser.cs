using PetClub.AppService.ViewModels.Account;
using PetClub.AppService.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.UserAppService
{
    public interface IAppServiceUser
    {
        Task AcceptTermsOfUse(string idUser);
        Task<List<GetUserByIdViewModel>> GetAllUsers(bool isActive, bool isAdmin, bool isPartner);
        Task<List<GetUserByIdViewModel>> GetAllUsersFilter(bool isActive, bool isAdmin, bool isPartner, string value);
        Task<UserBasicViewModel> GetByCpf(string Cpf);
        Task<GetUserByIdViewModel> GetByIdAsync(string Id);
        Task<GetUsersAmountViewModel> TotalUsersAmount();
        Task<GetUserByIdViewModel> UpdateAdmin(UserAdminUpdateViewModel user, string idAdmin);
        Task<UserUpdateViewModel> UpdateAsync(UpdatePerfilUserViewModel updatePerfilUserView, string IdUser);
    }
}
