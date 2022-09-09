using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PetClub.AppService.AppServices.CashFlowAppService;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.PurchaseOrderItemAppService;
using PetClub.AppService.ViewModels.PurchaseOrder;
using PetClub.Domain.Entities;
using PetClub.Domain.Enum;
using PetClub.Domain.Extensions;
using PetClub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.AppService.AppServices.UsersPartnersAppService;
using PetClub.AppService.AppServices.PaymentMethodAppService;
using System.Globalization;
using PetClub.AppService.ViewModels.PurchaseOrderItem;
using PetClub.AppService.Extensions;

namespace PetClub.AppService.AppServices.PurchaseOrderAppService
{
    public class AppServicePurchaseOrder : IAppServicePurchaseOrder
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotifierAppService _notifier;
        private readonly IAppServicePurchaseOrderItem _appServicePurchaseOrderItem;
        private readonly IAppServiceUsersPartners _appServiceUsersPartners;
        private readonly IAppServicePaymentMethod _appServicePaymentMethod;

        public AppServicePurchaseOrder(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            INotifierAppService notifier, 
            IAppServicePurchaseOrderItem appServicePurchaseOrderItem, 
            IAppServiceUsersPartners appServiceUsersPartners,
            IAppServicePaymentMethod appServicePaymentMethod)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
            _appServicePurchaseOrderItem = appServicePurchaseOrderItem;
            _appServiceUsersPartners = appServiceUsersPartners;
            _appServicePaymentMethod = appServicePaymentMethod;
        }

        public async Task<string> CreatePurchaseOrder(CreatePurchaseOrderViewModel model, string idParter)
        {
            var idOrder = await _unitOfWork.IRepositoryPurchaseOrder.AddReturnIdAsync(new PurchaseOrder(idParter, model.IdUser, model.IdPet, model.IdPaymentMethod, model.FullName, model.Cpf, model.Email, PurchaseOrderSituation.PENDING, PaymentSituation.PENDING, model.Observations, DateTime.Now.ToBrasilia()));
            await _unitOfWork.CommitAsync();
            return idOrder;
        }

        public async Task DeletePurchaseOrder(string idPurchaseOrder)
        {
            var order = await _unitOfWork.IRepositoryPurchaseOrder.GetByIdAsync(x => x.Id.Equals(idPurchaseOrder));
            if (order == null)
            {
                _notifier.Handle(new NotificationMessage("erro", "Pedido não encontrado."));
                throw new Exception();
            }
            if (order.PaymentSituation == PaymentSituation.APPROVED || order.PurchaseOrderSituation == PurchaseOrderSituation.CONCLUDED)
            {
                _notifier.Handle(new NotificationMessage("erro", "Não é possivel deletar uma compra que já foi paga e/ou finalizada."));
                throw new Exception();
            }
            if (order.PaymentSituation == PaymentSituation.CANCELED)
            {
                _notifier.Handle(new NotificationMessage("erro", "Não é possivel deletar uma compra que já foi cancelada."));
                throw new Exception();
            }

            order.RecordSituation = RecordSituation.INACTIVE;
            await _unitOfWork.IRepositoryPurchaseOrder.UpdateAsync(order);
            var orderItens = await _unitOfWork.IRepositoryPurchaseOrderItem.GetByOrderAsync(x => x.IdPurchaseOrder.Equals(idPurchaseOrder) && x.RecordSituation == RecordSituation.ACTIVE, x => x.DateCreation, false);
            foreach (var item in orderItens)
            {
                item.RecordSituation = RecordSituation.INACTIVE;
                await _unitOfWork.IRepositoryPurchaseOrderItem.UpdateAsync(item);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdadePurchaseOrder(UpdatePurchaseOrderViewModel model)
        {
            var order = await _unitOfWork.IRepositoryPurchaseOrder.GetByIdAsync(x => x.Id.Equals(model.IdPurchaseOrder));
            if (order == null)
            {
                _notifier.Handle(new NotificationMessage("erro", "Pedido não encontrado."));
                throw new Exception();
            }

            order.PurchaseOrderSituation = model.PurchaseOrderSituation;
            order.PaymentSituation = model.PaymentSituation;
            order.IdPaymentMethod = model.IdPaymentMethod;
            order.IdPet = model.IdPet;
            order.Cpf = model.Cpf;
            order.FullName = model.FullName;
            order.Email = model.FullName;
            order.IdUser = model.IdUser;
            order.WriteDate = DateTime.Now.ToBrasilia();
            await _unitOfWork.IRepositoryPurchaseOrder.UpdateAsync(order);

            foreach (var item in model.PurchaseOrderItem)
            {
                await _appServicePurchaseOrderItem.UpdadeOrDeleteItem(item, false);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task<GetPurchaseOrderViewModel> GetPurchaseOrderById(string idPurchaseOrder)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> include = t => t.Include(a => a.PaymentMethod).Include(b => b.User);
            var order = await _unitOfWork.IRepositoryPurchaseOrder.GetByIdAsync(x => x.Id.Equals(idPurchaseOrder));
            var orderItens = await _appServicePurchaseOrderItem.GetOrderItens(idPurchaseOrder);
            var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(order.IdPartner));
            var payment = _appServicePaymentMethod.GetPaymentType(order.PaymentMethod.PaymentType);
            var purchaseOrderSituation = GetPurchaseOrderSituation(order.PurchaseOrderSituation);
            var paymentSituation = GetPaymentSituation(order.PaymentSituation);

            var petName = "";
            if (!string.IsNullOrEmpty(order.IdPet))
            {
                var pet = await _unitOfWork.IRepositoryPet.GetByIdAsync(x => x.Id.Equals(order.IdPet));
                petName = pet.Name;
            }

            return new GetPurchaseOrderViewModel(idPurchaseOrder, order.IdPartner, partner.FullName, order.IdPet, petName, 
                order.IdPaymentMethod, payment, order.User.Id, order.FullName, order.Cpf, order.Email, purchaseOrderSituation, paymentSituation, 
                order.Observations, orderItens, order.WriteDate.ToString("d", culture), order.DateCreation.ToString("d", culture));
        }

        public async Task<List<GetPurchaseOrderViewModel>> GetPurchaseOrdersUser(string idUser, bool isApp)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetPurchaseOrderViewModel>();
            Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> include = t => t.Include(a => a.PaymentMethod).Include(b => b.User);
            IList<PurchaseOrder> orders = new List<PurchaseOrder>();
            if (isApp)
            {
                orders = await _unitOfWork.IRepositoryPurchaseOrder.GetByOrderAsync(x => x.IdUser.Equals(idUser), x => x.DateCreation, false);
            }
            else
            {
                orders = await _unitOfWork.IRepositoryPurchaseOrder.GetByOrderAsync(x => x.IdPartner.Equals(idUser), x => x.DateCreation, false);
            }

            foreach (var order in orders)
            {
                var orderItens = await _appServicePurchaseOrderItem.GetOrderItens(order.Id);
                var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(order.IdPartner));
                var payment = _appServicePaymentMethod.GetPaymentType(order.PaymentMethod.PaymentType);
                var purchaseOrderSituation = GetPurchaseOrderSituation(order.PurchaseOrderSituation);
                var paymentSituation = GetPaymentSituation(order.PaymentSituation);

                var petName = "";
                if (!string.IsNullOrEmpty(order.IdPet))
                {
                    var pet = await _unitOfWork.IRepositoryPet.GetByIdAsync(x => x.Id.Equals(order.IdPet));
                    petName = pet.Name;
                }

                list.Add(new GetPurchaseOrderViewModel(order.Id, order.IdPartner, partner.FullName, order.IdPet, petName,
                    order.IdPaymentMethod, payment, order.User.Id, order.FullName, order.Cpf, order.Email, purchaseOrderSituation, paymentSituation,
                    order.Observations, orderItens, order.WriteDate.ToString("d", culture), order.DateCreation.ToString("d", culture)));
            }
            return list;            
        }

        public string GetPurchaseOrderSituation(PurchaseOrderSituation situation)
        {
            var result = "";
            switch (situation)
            {
                case PurchaseOrderSituation.PENDING:
                    result = "Pendente";
                    break;
                case PurchaseOrderSituation.CONCLUDED:
                    result = "Concluído";
                    break;
                case PurchaseOrderSituation.CANCELED:
                    result = "Cancelado";
                    break;
            }
            return result;
        }

        public string GetPaymentSituation(PaymentSituation situation)
        {
            var result = "";
            switch (situation)
            {
                case PaymentSituation.PENDING:
                    result = "Pendente";
                    break;
                case PaymentSituation.APPROVED:
                    result = "Aprovado";
                    break;
                case PaymentSituation.CANCELED:
                    result = "Cancelado";
                    break;
                case PaymentSituation.VOID:
                    result = "Estornado";
                    break;
            }
            return result;
        }
    }
}
