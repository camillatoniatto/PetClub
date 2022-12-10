using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetClub.AppService.AppServices.CashFlowAppService;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.ViewModels.UsersPartners;
using PetClub.Domain.Entities;
using PetClub.Domain.Enum;
using PetClub.Domain.Extensions;
using PetClub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.AppService.AppServices.UserAppService;
using System.Globalization;
using System.Drawing;

namespace PetClub.AppService.AppServices.UsersPartnersAppService
{
    public class AppServiceUsersPartners : IAppServiceUsersPartners
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotifierAppService _notifier;
        private readonly IAppServiceUser _appServiceUser;

        public AppServiceUsersPartners(IUnitOfWork unitOfWork, IMapper mapper, INotifierAppService notifier, IAppServiceUser appServiceUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
            _appServiceUser = appServiceUser;
        }

        public async Task<string> CreateUsersPartners(string idUser, string idPartner)
        {
            var userPartner = await _unitOfWork.IRepositoryUsersPartners.GetByIdAsync(x => x.IdPartner.Equals(idPartner) && x.IdUser.Equals(idUser));
            if (userPartner != null)
            {
                _notifier.Handle(new NotificationMessage("Erro", "Esse cliente já foi incluido em sua lista anteriormente."));
                throw new Exception("Esse cliente já foi incluido em sua lista anteriormente.");
            }
            var id = "";
            if (userPartner == null)
            {
                id = await _unitOfWork.IRepositoryUsersPartners.AddReturnIdAsync(new UsersPartners(idPartner, idUser, DateTime.Now.ToBrasilia()));
                await _unitOfWork.CommitAsync();
            }
            else
            {
                if (userPartner.RecordSituation == RecordSituation.INACTIVE)
                {
                    userPartner.RecordSituation = RecordSituation.ACTIVE;
                    userPartner.WriteDate = DateTime.Now.ToBrasilia();
                    await _unitOfWork.IRepositoryUsersPartners.UpdateAsync(userPartner);
                    await _unitOfWork.CommitAsync();
                }
                id = userPartner.Id;
            }
            return id;
        }

        public async Task<List<GetUsersPartnersViewModel>> GetUsersPartners(string idPartner)
        {
            CultureInfo culture = new CultureInfo("pt-BR");

            var list = new List<GetUsersPartnersViewModel>();
            Func<IQueryable<UsersPartners>, IIncludableQueryable<UsersPartners, object>> include = t => t.Include(a => a.User).ThenInclude(x => x.Pet);
            var userPartners = await _unitOfWork.IRepositoryUsersPartners.GetByOrderAsync(x => x.IdPartner.Equals(idPartner) && x.RecordSituation == RecordSituation.ACTIVE, x => x.User.FullName, false, include);
            var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(idPartner));
            foreach (var item in userPartners)
            {
                var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(item.IdUser));
                var pets = await _unitOfWork.IRepositoryPet.GetByAsync(x => x.IdUser.Equals(item.IdUser));
                list.Add(new GetUsersPartnersViewModel(item.Id, item.IdUser, item.User.FullName, item.User.Cpf, item.User.Email, item.User.PhoneNumber, partner.Id, partner.FullName, partner.Cpf, item.DateCreation.ToString("d", culture), 
                                                       pets.Count(), user.Birthdate.ToString("d", culture), user.AddressName, user.Number, user.Complement, user.Neighborhood, user.City, user.State, user.ZipCode));
            }
            return list;

        }

        public async Task<GetUsersPartnersViewModel> GetByIdUsersPartners(string idUsersPartners)
        {
            CultureInfo culture = new CultureInfo("pt-BR");

            Func<IQueryable<UsersPartners>, IIncludableQueryable<UsersPartners, object>> include = t => t.Include(a => a.User).ThenInclude(x => x.Pet);
            var userPartners = await _unitOfWork.IRepositoryUsersPartners.GetByIdAsync(x => x.Id.Equals(idUsersPartners), include);
            var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(userPartners.IdPartner));
            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(userPartners.IdUser));
            var pets = await _unitOfWork.IRepositoryPet.GetByAsync(x => x.IdUser.Equals(user.Id));
            return new GetUsersPartnersViewModel(idUsersPartners, user.Id, user.FullName, user.Cpf, user.Email, user.PhoneNumber, partner.Id, partner.FullName, partner.Cpf, userPartners.DateCreation.ToString("d", culture), 
                                                pets.Count(), user.Birthdate.ToString("d", culture), user.AddressName, user.Number, user.Complement, user.Neighborhood, user.City, user.State, user.ZipCode);
        }

        public async Task DeleteUsersPartners(string idUsersPartners)
        {
            var userPartner = await _unitOfWork.IRepositoryUsersPartners.GetByIdAsync(x => x.Id.Equals(idUsersPartners));
            if (userPartner != null)
            {
                userPartner.RecordSituation = RecordSituation.INACTIVE;
                await _unitOfWork.CommitAsync();
            }
            else
            {
                _notifier.Handle(new NotificationMessage("userPartner", "NotFound"));
                throw new Exception();
            }
        }
    }
}
