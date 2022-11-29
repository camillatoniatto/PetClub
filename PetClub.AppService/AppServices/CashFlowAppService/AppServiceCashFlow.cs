using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.ViewModels.CashFlow;
using PetClub.Domain.Entities;
using PetClub.Domain.Enum;
using PetClub.Domain.Extensions;
using PetClub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.AppService.ViewModels.PurchaseOrder;
using System.Globalization;

namespace PetClub.AppService.AppServices.CashFlowAppService
{
    public class AppServiceCashFlow : IAppServiceCashFlow
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotifierAppService _notifier;

        public AppServiceCashFlow(IUnitOfWork unitOfWork, IMapper mapper, INotifierAppService notifier)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
        }

        public async Task CreateReceivableBill(CreateCashFlowViewModel model, string idUser)
        {
            try
            {
                decimal netValue = 0M;
                var payment = await _unitOfWork.IRepositoryPaymentMethod.GetByIdAsync(x => x.Id.Equals(model.IdPaymentMethod));
                if (payment != null)
                {
                    netValue = model.LaunchValue;
                    if (!string.IsNullOrEmpty(model.IdPurchaseOrder))
                    {
                        netValue = model.LaunchValue - (model.LaunchValue * payment.AdminTax / 100);
                    }
                }

                CashFlow cashFlow;
                for (int i = 1; i <= payment.NumberInstallments; i++)
                {
                    var value = netValue / payment.NumberInstallments;
                    DateTime dateTypePayment = model.ExpirationDate;
                    if (payment.PaymentType == PaymentType.CREDIT_CARD)
                    {
                        dateTypePayment = DateTime.Now.ToBrasilia().AddDays(30 * i);
                    }

                    string userWriteOff = "";
                    if (model.WriteOffDate != DateTime.MinValue)
                        userWriteOff = idUser;

                    cashFlow = new CashFlow(model.Title, model.Description, idUser, model.IdPurchaseOrder, model.IdPaymentMethod, model.LaunchValue, 
                                    netValue, dateTypePayment, model.WriteOffDate, userWriteOff, "", model.isOutflow, DateTime.Now.ToBrasilia());
                    await _unitOfWork.IRepositoryCashFlow.AddAsync(cashFlow);

                }

                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                _notifier.Handle(new NotificationMessage("Erro", "Erro ao gerar uma conta!"));
                throw new Exception("Erro ao gerar uma conta!");
            }
        }

        public async Task<List<GetCashFlowViewModel>> GetCashFlow(string idUser)
        {
            var list = new List<GetCashFlowViewModel>();
            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(idUser));
            if (user.IsPartner)
            {
                list = await GetCashFlowPartner(idUser);
            }
            else
            {
                list = await GetCashFlowAdmin();
            }
            return list;

        }

        public async Task<List<GetCashFlowViewModel>> GetCashFlowPartner(string idPARTNER)
        {
            var list = new List<GetCashFlowViewModel>();
            CultureInfo culture = new CultureInfo("pt-BR");
            Func<IQueryable<CashFlow>, IIncludableQueryable<CashFlow, object>> include = t => t.Include(a => a.PaymentMethod);
            var cashflow = await _unitOfWork.IRepositoryCashFlow.GetByOrderAsync(x => x.IdUserCreate.Equals(idPARTNER) && x.RecordSituation.Equals(RecordSituation.ACTIVE), x => x.DateCreation, false, include);
            var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(idPARTNER));

            foreach (var item in cashflow)
            {
                var order = await _unitOfWork.IRepositoryPurchaseOrder.GetByIdAsync(x => x.Id.Equals(item.IdPurchaseOrder));
                var payment = GetPaymentType(item.PaymentMethod.PaymentType, item.PaymentMethod.NumberInstallments);

                string status = "Pendente";
                if (!string.IsNullOrEmpty(item.IdPurchaseOrder) && order.PaymentSituation == PaymentSituation.CANCELED)
                    status = "Cancelado";
                else
                {
                    var date = DateTime.Now.ToBrasilia().Date;
                    if (item.WriteOffDate != DateTime.MinValue)
                    {
                        if (item.isOutflow)
                        {
                            status = "Pago";
                        }
                        else
                        {
                            status = "Recebido";
                        }
                    }
                    else if (item.WriteOffDate == DateTime.MinValue && date > item.ExpirationDate)
                    {
                        status = "Em atraso";
                    }
                }

                var userCreate = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(item.IdUserCreate));

                list.Add(new GetCashFlowViewModel(item.Id, status, item.Title, item.Description, item.IdUserCreate, userCreate.FullName, item.IdPurchaseOrder, item.IdPaymentMethod, payment, item.LaunchValue,
                                                  item.NetValue, item.ExpirationDate.ToString("d", culture), item.WriteOffDate != DateTime.MinValue ? item.WriteOffDate.ToString("d", culture) : null,
                                                  item.IdUserWriteOff, userCreate.FullName, item.IdUserInactivate, null, item.isOutflow, item.WriteDate));
            }
            return list;
        }

        public async Task<List<GetCashFlowViewModel>> GetCashFlowAdmin()
        {
            var list = new List<GetCashFlowViewModel>();
            CultureInfo culture = new CultureInfo("pt-BR");
            Func<IQueryable<CashFlow>, IIncludableQueryable<CashFlow, object>> include = t => t.Include(a => a.PaymentMethod);
            var cashflow = await _unitOfWork.IRepositoryCashFlow.GetByOrderAsync(x => x.RecordSituation.Equals(RecordSituation.ACTIVE), x => x.DateCreation, false, include);
            foreach (var item in cashflow)
            {
                var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(item.IdUserCreate));
                var order = await _unitOfWork.IRepositoryPurchaseOrder.GetByIdAsync(x => x.Id.Equals(item.IdPurchaseOrder));
                var payment = GetPaymentType(item.PaymentMethod.PaymentType, item.PaymentMethod.NumberInstallments);

                string status = "Pendente";
                if (!string.IsNullOrEmpty(item.IdPurchaseOrder) && order.PaymentSituation == PaymentSituation.CANCELED)
                    status = "Cancelado";
                else
                {
                    var date = DateTime.Now.ToBrasilia().Date;
                    if (item.WriteOffDate != DateTime.MinValue)
                    {
                        if (item.isOutflow)
                        {
                            status = "Pago";
                        }
                        else
                        {
                            status = "Recebido";
                        }
                    }
                    else if (item.WriteOffDate == DateTime.MinValue && date > item.ExpirationDate)
                    {
                        status = "Em atraso";
                    }
                }

                var userCreate = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(item.IdUserCreate));

                list.Add(new GetCashFlowViewModel(item.Id, status, item.Title, item.Description, item.IdUserCreate, userCreate.FullName, item.IdPurchaseOrder, item.IdPaymentMethod, payment, item.LaunchValue,
                                                  item.NetValue, item.ExpirationDate.ToString("d", culture), item.WriteOffDate != DateTime.MinValue ? item.WriteOffDate.ToString("d", culture) : null,
                                                  item.IdUserWriteOff, userCreate.FullName, item.IdUserInactivate, null, item.isOutflow, item.WriteDate));
            }
            return list;
        }

        public async Task<UpdateCashFlowViewModel> UpdateAsync(UpdateCashFlowViewModel model, string idUser)
        {
            var bill = await _unitOfWork.IRepositoryCashFlow.GetByIdAsync(x => x.Id.Equals(model.IdCashFlow));
            try
            {
                if (!bill.isOutflow && bill.IdPurchaseOrder != null)
                {
                    _notifier.Handle(new NotificationMessage("Erro", "Não é possível editar uma entrada de caixa"));
                    throw new Exception("Não é possível editar uma entrada de caixa");
                }

                if (bill.WriteOffDate != DateTime.MinValue)
                {
                    decimal netValue = 0M;
                    var payment = await _unitOfWork.IRepositoryPaymentMethod.GetByIdAsync(x => x.Id.Equals(model.IdPaymentMethod));
                    if (payment != null)
                    {
                        netValue = model.LaunchValue - (model.LaunchValue * payment.AdminTax / 100);
                    }

                    var value = netValue / payment.NumberInstallments;
                    bill.Title = model.Title;
                    bill.IdPaymentMethod = model.IdPaymentMethod;
                    bill.ExpirationDate = model.ExpirationDate;
                    bill.LaunchValue = model.LaunchValue;
                    bill.NetValue = value;

                    if (bill.WriteOffDate != DateTime.MinValue && model.WriteOffDate != DateTime.MinValue)
                    {
                        bill.WriteOffDate = model.WriteOffDate;
                        bill.IdUserWriteOff = idUser;
                    }

                    var upd = await _unitOfWork.IRepositoryCashFlow.UpdateAsync(bill);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    _notifier.Handle(new NotificationMessage("Erro", "Não é possível editar uma conta já baixada!"));
                    throw new Exception("Não é possível editar uma conta já baixada!");
                }
            }
            catch (Exception)
            {
                _notifier.Handle(new NotificationMessage("Erro", "Erro ao atualizar essa conta!"));
                throw new Exception("Erro ao atualizar essa conta!");

            }
            return model;

        }

        public async Task WriteOff(string idCashFlow, string idUser)
        {
            try
            {
                var bills = await _unitOfWork.IRepositoryCashFlow.GetByIdAsync(x => x.Id.Equals(idCashFlow));
                bills.WriteOffDate = DateTime.Now.ToBrasilia();
                bills.IdUserWriteOff = idUser;

                if (!string.IsNullOrEmpty(bills.IdPurchaseOrder))
                {
                    var purchaseOrder = await _unitOfWork.IRepositoryPurchaseOrder.GetByIdAsync(x => x.Id.Equals(bills.IdPurchaseOrder));
                    purchaseOrder.PurchaseOrderSituation = PurchaseOrderSituation.CONCLUDED;
                    purchaseOrder.PaymentSituation = PaymentSituation.APPROVED;
                    await _unitOfWork.IRepositoryPurchaseOrder.UpdateAsync(purchaseOrder);
                }

                await _unitOfWork.IRepositoryCashFlow.UpdateAsync(bills);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                _notifier.Handle(new NotificationMessage("Erro", "Erro ao dar baixa!"));
                throw new Exception("Erro ao dar baixa!");
            }
        }

        public async Task DeleteBill(string idCashFlow, string idUser)
        {
            try
            {
                var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(idUser));
                var Bills = await _unitOfWork.IRepositoryCashFlow.GetByIdAsync(x => x.Id.Equals(idCashFlow));
                if (Bills == null)
                {
                    _notifier.Handle(new NotificationMessage("Erro", "Não é possível encontrar essa conta."));
                    throw new Exception("Não é possível encontrar essa conta.");
                }
                if (Bills.IdUserWriteOff != null && !user.IsAdmin)
                {
                    _notifier.Handle(new NotificationMessage("Erro", "Não é possível deletar uma conta que já foi dado baixa."));
                    throw new Exception("Não é possível deletar uma conta que já foi dado baixa.");
                }

                if (Bills.IdPurchaseOrder != null)
                {
                    //Func<IQueryable<PurchaseOrder>, IIncludableQueryable<PurchaseOrder, object>> include = t => t.Include(a => a.PurchaseOrderItem);
                    var order = await _unitOfWork.IRepositoryPurchaseOrder.GetByIdAsync(x => x.Id.Equals(Bills.IdPurchaseOrder));
                    order.PaymentSituation = PaymentSituation.CANCELED;
                    order.PurchaseOrderSituation = PurchaseOrderSituation.CANCELED;
                    await _unitOfWork.IRepositoryPurchaseOrder.UpdateAsync(order);
                }

                Bills.RecordSituation = RecordSituation.INACTIVE;
                Bills.IdUserInactivate = idUser;
                await _unitOfWork.IRepositoryCashFlow.UpdateAsync(Bills);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                _notifier.Handle(new NotificationMessage("Erro", e.Message));
                throw new Exception(e.Message);
            }
            
        }

        public async Task<ResumeCashflowViewModel> ResumeCashFlow(string idUser)
        {
            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(idUser));
            IList<CashFlow> cashflow = new List<CashFlow>();
            var list = await GetCashFlow(idUser);

            var entrada = 0M;
            var saida = 0M;
            var saldo = 0M;
            foreach (var bill in list)
            {
                if (bill.IsOutflow)
                {
                    saida += bill.LaunchValue;
                }
                else
                {
                    entrada += bill.LaunchValue;
                }
            }
            saldo = entrada - saida;
            return new ResumeCashflowViewModel(entrada.ToString("F2"), saida.ToString("F2"), saldo.ToString("F2"));
        }

        public string GetPaymentType(PaymentType paymentType, int installment)
        {
            var result = "";
            switch (paymentType)
            {
                case PaymentType.CREDIT_CARD:
                    result = "Cartão de crédito em "+installment+"X";
                    break;
                case PaymentType.CASH:
                    result = "Dinheiro";
                    break;
                case PaymentType.DEBIT:
                    result = "Cartão de débito";
                    break;
                case PaymentType.PIX:
                    result = "Pix";
                    break;
            }
            return result;
        }
    }
}
