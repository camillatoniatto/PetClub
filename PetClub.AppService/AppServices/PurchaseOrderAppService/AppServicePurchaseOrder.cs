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
using PetClub.AppService.ViewModels.CashFlow;

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
        private readonly IAppServiceCashFlow _appServiceCashFlow;

        public AppServicePurchaseOrder(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            INotifierAppService notifier, 
            IAppServicePurchaseOrderItem appServicePurchaseOrderItem, 
            IAppServiceUsersPartners appServiceUsersPartners,
            IAppServicePaymentMethod appServicePaymentMethod,
            IAppServiceCashFlow appServiceCashFlow)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
            _appServicePurchaseOrderItem = appServicePurchaseOrderItem;
            _appServiceUsersPartners = appServiceUsersPartners;
            _appServicePaymentMethod = appServicePaymentMethod;
            _appServiceCashFlow = appServiceCashFlow;
        }

        public async Task<string> CreatePurchaseOrder(CreatePurchaseOrderViewModel model, string idPartner)
        {
            var client = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(model.IdUser));
            var idOrder = await _unitOfWork.IRepositoryPurchaseOrder.AddReturnIdAsync(new PurchaseOrder(idPartner, model.IdUser, model.IdPet, model.IdPaymentMethod, client.FullName, client.Cpf, client.Email, PurchaseOrderSituation.CONCLUDED, PaymentSituation.APPROVED, model.Observations, DateTime.Now.ToBrasilia()));
            foreach (var item in model.PurchaseOrderItens)
            {
                var orderItem = new CreatePurchaseOrderItemViewModel
                {
                    IdPurchaseOrder = idOrder,
                    IdService = item.IdService,
                    Quantity = item.Quantity
                };
                await _appServicePurchaseOrderItem.AddPurchaseOrderItem(orderItem);
            }

            await _unitOfWork.CommitAsync();

            var launchValue = 0M;

            if (!string.IsNullOrEmpty(idOrder))
            {
                var orderItens = await _unitOfWork.IRepositoryPurchaseOrderItem.GetByAsync(x => x.IdPurchaseOrder.Equals(idOrder));
                var date = DateTime.Now.ToBrasilia();
                var listService = new List<string>();
                foreach (var item in orderItens.Where(x => x.Quantity > 0))
                {
                    listService.Add(item.Service.Title);
                    launchValue += item.Value * item.Quantity;
                }
                var serviceTitles = String.Join(", ", listService);
                var description = "Serviços: " + serviceTitles;
                CreateCashFlowViewModel cashflow = new CreateCashFlowViewModel("Venda: " + idOrder, description, idPartner, idOrder,
                                                                               model.IdPaymentMethod, launchValue, date, date, false);


                await _appServiceCashFlow.CreateReceivableBill(cashflow, idPartner);
                await _unitOfWork.CommitAsync();
            }


            return idOrder;
        }

        public async Task ConcluedPurchaseOrder(string idPurchaseOrder, bool isPaid)
        {
            var date = DateTime.Now.ToBrasilia();
            var listService = new List<string>();
            Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> include = t => t.Include(a => a.PurchaseOrderItem).ThenInclude(b => b.Service);
            var order = await _unitOfWork.IRepositoryPurchaseOrder.GetByIdAsync(x => x.Id.Equals(idPurchaseOrder), include);
            if (order == null)
            {
                _notifier.Handle(new NotificationMessage("Erro", "Não foi possível encontrar essa compra."));
                throw new Exception("Não foi possível encontrar essa compra.");
            }
            if (order.PurchaseOrderItem.Count() == 0)
            {
                _notifier.Handle(new NotificationMessage("Erro", "Parece que essa compra ainda não tem serviços cadastrados."));
                throw new Exception("Parece que essa compra ainda não tem serviços cadastrados.");
            }

            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(order.IdUser));
            var launchValue = 0M;
            foreach (var item in order.PurchaseOrderItem.Where(x => x.RecordSituation.Equals(RecordSituation.ACTIVE)))
            {
                listService.Add(item.Service.Title);
                launchValue += item.Value * item.Quantity;
            }
            var serviceTitles = String.Join(", ", listService);
            var description = "Serviços prestados: "+ serviceTitles;
            CreateCashFlowViewModel cashflow = new CreateCashFlowViewModel("Venda de serviço - " + user.FullName, description, order.IdPartner, idPurchaseOrder,
                                                                           order.IdPaymentMethod, launchValue, date, isPaid ? date : DateTime.MinValue, false);


            await _appServiceCashFlow.CreateReceivableBill(cashflow, order.IdPartner);
            if (isPaid)
            {
                order.PurchaseOrderSituation = PurchaseOrderSituation.CONCLUDED;
                order.PaymentSituation = PaymentSituation.APPROVED;
                await _unitOfWork.IRepositoryPurchaseOrder.UpdateAsync(order);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task DeletePurchaseOrder(string idPurchaseOrder)
        {
            var order = await _unitOfWork.IRepositoryPurchaseOrder.GetByIdAsync(x => x.Id.Equals(idPurchaseOrder));
            var bill = await _unitOfWork.IRepositoryCashFlow.GetByIdAsync(x => x.IdPurchaseOrder.Equals(idPurchaseOrder));

            if (order == null)
            {
                _notifier.Handle(new NotificationMessage("erro", "Pedido não encontrado."));
                throw new Exception("Pedido não encontrado.");
            }
            if (order.PaymentSituation == PaymentSituation.CANCELED)
            {
                _notifier.Handle(new NotificationMessage("erro", "Não é possivel cancelar uma compra que já foi cancelada."));
                throw new Exception("Não é possivel cancelar uma compra que já foi cancelada.");
            }
            bill.RecordSituation = RecordSituation.INACTIVE;
            order.PurchaseOrderSituation = PurchaseOrderSituation.CANCELED;
            await _unitOfWork.IRepositoryPurchaseOrder.UpdateAsync(order);
            await _unitOfWork.IRepositoryCashFlow.UpdateAsync(bill);
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
                throw new Exception("Pedido não encontrado.");
            }

            order.PurchaseOrderSituation = model.PurchaseOrderSituation;
            order.PaymentSituation = model.PaymentSituation;
            order.IdPaymentMethod = model.IdPaymentMethod;
            //order.IdPet = model.IdPet;
            //order.Cpf = model.Cpf;
            //order.FullName = model.FullName;
            //order.Email = model.Email;
            //order.IdUser = model.IdUser;
            order.WriteDate = DateTime.Now.ToBrasilia();
            await _unitOfWork.IRepositoryPurchaseOrder.UpdateAsync(order);

            foreach (var item in model.PurchaseOrderItem)
            {
                await _appServicePurchaseOrderItem.UpdadeOrDeleteItem(item, false);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task<string> GetValue(List<CreatePurchaseOrderItemViewModel> itens)
        {
            var finalValue = 0M;
            foreach (var item in itens.Where(x => x.Quantity != 0))
            {
                var service = await _unitOfWork.IRepositoryService.GetByIdAsync(x => x.Id.Equals(item.IdService));
                finalValue += service.Value * item.Quantity;
            }
            return finalValue.ToString("F2");
        }

        public async Task<GetPurchaseOrderViewModel> GetPurchaseOrderById(string idPurchaseOrder)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> include = t => t.Include(a => a.PaymentMethod);
            var order = await _unitOfWork.IRepositoryPurchaseOrder.GetByIdAsync(x => x.Id.Equals(idPurchaseOrder), include);
            var orderItens = await _appServicePurchaseOrderItem.GetOrderItens(idPurchaseOrder);
            var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(order.IdPartner));
            var payment = _appServicePaymentMethod.GetPaymentType(order.PaymentMethod.PaymentType, order.PaymentMethod.NumberInstallments);
            var purchaseOrderSituation = GetPurchaseOrderSituation(order.PurchaseOrderSituation);
            var paymentSituation = GetPaymentSituation(order.PaymentSituation);

            var petName = "";
            if (!string.IsNullOrEmpty(order.IdPet))
            {
                var pet = await _unitOfWork.IRepositoryPet.GetByIdAsync(x => x.Id.Equals(order.IdPet));
                petName = pet.Name;
            }

            var total = 0M;
            foreach (var item in orderItens)
            {
                total += item.Quantity * item.Value;
            }

            return new GetPurchaseOrderViewModel(idPurchaseOrder, order.IdPartner, partner.FullName, order.IdPet, petName, 
                order.IdPaymentMethod, payment, order.IdUser, order.FullName, order.Cpf, order.Email, purchaseOrderSituation, paymentSituation, 
                order.Observations, total.ToString("F2"), order.WriteDate.ToString("d", culture), order.DateCreation.ToString("d", culture), orderItens);
        }

        public async Task<List<GetPurchaseOrderViewModel>> GetPurchaseOrdersUser(string idUser, bool isApp)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetPurchaseOrderViewModel>();
            Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> include = t => t.Include(a => a.PaymentMethod);
            IList<PurchaseOrder> orders = new List<PurchaseOrder>();
            if (isApp)
            {
                orders = await _unitOfWork.IRepositoryPurchaseOrder.GetByOrderAsync(x => x.IdUser.Equals(idUser), x => x.DateCreation, false, include);
            }
            else
            {
                orders = await _unitOfWork.IRepositoryPurchaseOrder.GetByOrderAsync(x => x.IdPartner.Equals(idUser), x => x.DateCreation, false, include);
            }

            foreach (var order in orders)
            {
                var orderItens = await _appServicePurchaseOrderItem.GetOrderItens(order.Id);
                var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(order.IdPartner));
                var payment = _appServicePaymentMethod.GetPaymentType(order.PaymentMethod.PaymentType, order.PaymentMethod.NumberInstallments);
                var purchaseOrderSituation = GetPurchaseOrderSituation(order.PurchaseOrderSituation);
                var paymentSituation = GetPaymentSituation(order.PaymentSituation);

                var petName = "";
                if (!string.IsNullOrEmpty(order.IdPet))
                {
                    var pet = await _unitOfWork.IRepositoryPet.GetByIdAsync(x => x.Id.Equals(order.IdPet));
                    petName = pet.Name;
                }

                var total = 0M;
                foreach (var item in orderItens)
                {
                    total += item.Quantity * item.Value;
                }

                list.Add(new GetPurchaseOrderViewModel(order.Id, order.IdPartner, partner.FullName, order.IdPet, petName,
                    order.IdPaymentMethod, payment, order.IdUser, order.FullName, order.Cpf, order.Email, purchaseOrderSituation, paymentSituation,
                    order.Observations, total.ToString("F2"), order.WriteDate.ToString("d", culture), order.DateCreation.ToString("d", culture), orderItens));

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
                    result = "Concluido";
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
