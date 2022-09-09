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

namespace PetClub.AppService.AppServices.UsersPartnersAppService
{
    public class AppServiceUsersPartners : IAppServiceUsersPartners
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotifierAppService _notifier;

        public AppServiceUsersPartners(IUnitOfWork unitOfWork, IMapper mapper, INotifierAppService notifier)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
        }

        public async Task<string> CreateUsersPartners(string idUser, string idPartner)
        {
            var userPartner = await _unitOfWork.IRepositoryUsersPartners.GetByIdAsync(x => x.IdPartner.Equals(idPartner) && x.IdUser.Equals(idUser));
            var id = "";
            if (userPartner == null)
            {
                id = await _unitOfWork.IRepositoryUsersPartners.AddReturnIdAsync(new UsersPartners(idUser, idPartner, DateTime.Now.ToBrasilia()));
                await _unitOfWork.CommitAsync();
            }
            else
            {
                id = userPartner.Id;
            }
            return id;
        }

        public async Task<List<GetUsersPartnersViewModel>> GetUsersPartners(string idPartner)
        {
            var list = new List<GetUsersPartnersViewModel>();
            Func<IQueryable<UsersPartners>, IIncludableQueryable<UsersPartners, object>> include = t => t.Include(a => a.User).ThenInclude(x => x.Pet);
            var userPartners = await _unitOfWork.IRepositoryUsersPartners.GetByOrderAsync(x => x.IdPartner.Equals(idPartner) && x.RecordSituation == RecordSituation.ACTIVE, x => x.User.FullName, false);
            var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(idPartner));
            if (userPartners != null)
            {
                foreach (var item in userPartners)
                {
                    list.Add(new GetUsersPartnersViewModel(item.Id, item.IdUser, item.User.FullName, item.User.Cpf, item.User.Email, item.User.PhoneNumber, partner.Id, partner.FullName, partner.Cpf));
                }
                return list;
            }
            else
            {
                _notifier.Handle(new NotificationMessage("userPartner", "Não há clientes registrados."));
                throw new Exception();
            }
        }

        public async Task<GetUsersPartnersViewModel> GetByIdUsersPartners(string idUsersPartners)
        {
            Func<IQueryable<UsersPartners>, IIncludableQueryable<UsersPartners, object>> include = t => t.Include(a => a.User).ThenInclude(x => x.Pet);
            var userPartners = await _unitOfWork.IRepositoryUsersPartners.GetByIdAsync(x => x.Id.Equals(idUsersPartners));
            var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(userPartners.IdPartner));
            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(userPartners.IdUser));
            
            return new GetUsersPartnersViewModel(idUsersPartners, user.Id, user.FullName, user.Cpf, user.Email, user.PhoneNumber, partner.Id, partner.FullName, partner.Cpf);
        }

        //public async Task UpdateUsersPartners(UpdateGatewayBuyerViewModel model, string idGatewayBuyer, string idUser)
        //{
        //    var id_gateway_fk = await _unitOfWork.IRepositoryUsersPartners.GetByIdAsync(x => x.Id.Equals(idGatewayBuyer));
        //    if (id_gateway_fk != null)
        //    {
        //        var association = await _unitOfWork.IRepositoryUsersPartners.GetByIdAsync(x => x.Gtfk_Buyer_id.Equals(model.Gtfk_Buyer_id) && x.Id != idGatewayBuyer);
        //        if (association == null)
        //        {
        //            id_gateway_fk.Gtfk_Buyer_id = model.Gtfk_Buyer_id != null ? model.Gtfk_Buyer_id : id_gateway_fk.Gtfk_Buyer_id;
        //            id_gateway_fk.IdUser = model.IdUser != null ? model.IdUser : id_gateway_fk.IdUser;
        //            id_gateway_fk.IdGatewayKey = model.IdGatewayKey != null ? model.IdGatewayKey : id_gateway_fk.IdGatewayKey;
        //            id_gateway_fk.WriteUid = idUser;
        //            id_gateway_fk.WriteDate = DateTime.Now.ToBrasilia();

        //            await _unitOfWork.IRepositoryUsersPartners.UpdateAsync(id_gateway_fk);
        //            await _unitOfWork.CommitAsync();
        //        }
        //        else
        //        {
        //            _notifier.Handle(new NotificationMessage("GatewayBuyer", "Gtfk_Buyer_id já foi cadastrado com outro IdUser"));
        //            throw new Exception();
        //        }
        //    }
        //    else
        //    {
        //        _notifier.Handle(new NotificationMessage("GatewayBuyer", "IdGatewayBuyer não encontrado"));
        //        throw new Exception();
        //    }
        //}

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
