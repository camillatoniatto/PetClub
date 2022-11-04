using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PetClub.AppService.AppServices.CashFlowAppService;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.ViewModels.Pet;
using PetClub.AppService.ViewModels.PurchaseOrder;
using PetClub.AppService.ViewModels.PurchaseOrderItem;
using PetClub.Domain.Entities;
using PetClub.Domain.Enum;
using PetClub.Domain.Extensions;
using PetClub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.PurchaseOrderItemAppService
{
    public class AppServicePurchaseOrderItem : IAppServicePurchaseOrderItem
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotifierAppService _notifier;

        public AppServicePurchaseOrderItem(IUnitOfWork unitOfWork, IMapper mapper, INotifierAppService notifier)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
        }

        public async Task<string> AddPurchaseOrderItem(CreatePurchaseOrderItemViewModel model)
        {
            try
            {
                var service = await _unitOfWork.IRepositoryService.GetByIdAsync(x => x.Id.Equals(model.IdService));
                if (service == null)
                {
                    _notifier.Handle(new NotificationMessage("erro", "Serviço não encontrado."));
                    throw new Exception();
                }
                var idOrderItem = await _unitOfWork.IRepositoryPurchaseOrderItem.AddReturnIdAsync(new PurchaseOrderItem(model.IdPurchaseOrder, model.IdService, model.Quantity, service.Value));
                await _unitOfWork.CommitAsync();
                return idOrderItem;
            }
            catch (Exception e)
            {
                _notifier.Handle(new NotificationMessage("erro", e.Message));
                throw new Exception();
            }
        }

        public async Task UpdadeOrDeleteItem(UpdatePurchaseOrderItemViewModel model, bool upd)
        {
            var orderItem = await _unitOfWork.IRepositoryPurchaseOrderItem.GetByIdAsync(x => x.Id.Equals(model.IdPurchaseOrderItem));
            if (orderItem == null)
            {
                _notifier.Handle(new NotificationMessage("erro", "Item de pedido não encontrado."));
                throw new Exception();
            }
            var service = await _unitOfWork.IRepositoryService.GetByIdAsync(x => x.Id.Equals(model.IdService));
            if (service == null)
            {
                _notifier.Handle(new NotificationMessage("erro", "Serviço não encontrado."));
                throw new Exception();
            }

            if (!model.Delete)
            {
                orderItem.Quantity = model.Quantity;
                orderItem.IdService = service.Id;
                orderItem.Value = service.Value;
                await _unitOfWork.IRepositoryPurchaseOrderItem.UpdateAsync(orderItem);
            }
            else
            {
                orderItem.RecordSituation = RecordSituation.INACTIVE;
                await _unitOfWork.IRepositoryPurchaseOrderItem.UpdateAsync(orderItem);
            }

            if (upd)
            {
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<List<GetOrderItensViewModel>> GetOrderItens(string idPurchaseOrder)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetOrderItensViewModel>();
            Func<IQueryable<PurchaseOrderItem>, IIncludableQueryable<PurchaseOrderItem, object>> include = t => t.Include(a => a.Service);
            var orderItens = await _unitOfWork.IRepositoryPurchaseOrderItem.GetByOrderAsync(x => x.IdPurchaseOrder.Equals(idPurchaseOrder) && x.RecordSituation == RecordSituation.ACTIVE, x => x.DateCreation, false, include);
            foreach (var item in orderItens)
            {
                list.Add(new GetOrderItensViewModel(item.Id, item.IdPurchaseOrder, item.IdService, item.Service.Title, item.Service.Description, item.Quantity, item.Value, item.WriteDate.ToString("d", culture)));
            }
            return list;
        }
    }
}
