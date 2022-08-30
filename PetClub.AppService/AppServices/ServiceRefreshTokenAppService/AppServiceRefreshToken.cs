using PetClub.AppService.ViewModels.User;
using PetClub.AppService.ViewModels.Account;
using PetClub.Domain.Interfaces;
using PetClub.Domain.Entities;
using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.ServiceRefreshTokenAppService
{
    public class AppServiceRefreshToken : IAppServiceRefreshToken
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppServiceRefreshToken(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(RefreshTokenDataViewModel refreshTokenDataViewModel)
        {
            await _unitOfWork.IRepositoryRefreshTokenData.AddAsync(new RefreshTokenData(refreshTokenDataViewModel.IdUser, refreshTokenDataViewModel.RefreshToken));
            await _unitOfWork.CommitAsync();
        }

        public async Task<string> ValidateToken(LoginViewModel loginViewModel, string idUser)
        {
            var token = await _unitOfWork.IRepositoryRefreshTokenData.GetFirstWithOrderTypeAsync(x => x.IdUser.Equals(idUser) && x.RefreshToken.Equals(loginViewModel.RefreshToken), x => x.DateCreation, true);
            if (token == null) return null;
            token.RecordSituation = Domain.Enum.Enum.RecordSituation.INACTIVE;
            var inative = _unitOfWork.IRepositoryRefreshTokenData.UpdateAsync(token);
            await _unitOfWork.CommitAsync();

            var newToken = Guid.NewGuid().ToString();
            try
            {
                await Create(new RefreshTokenDataViewModel(idUser, newToken));
                await _unitOfWork.CommitAsync();
                return newToken;
            }
            catch
            {
                return null;
            }
        }
    }
}
