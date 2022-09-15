using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.PetAppService;
using PetClub.Domain.Extensions;
using PetClub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.DashboardAppService
{
    public class AppServiceDashboard : IAppServiceDashboard
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotifierAppService _notifier;

        public AppServiceDashboard(IUnitOfWork unitOfWork, IMapper mapper, INotifierAppService notifier)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
        }

        /*
         public async Task<DashViewModel> GetDash(string IdAthletic, int days)
        {
            var receivable = await _appServiceReceivableBills.GetMyAccountsReceive(IdAthletic);
            var pay = await _appServiceReceivablePay.GetMyReceivableMonth(IdAthletic);
            var associate = await _appServiceRequestAssociate.AmountAssociative(IdAthletic);
            var sales = await _appServiceOrder.DaySale(IdAthletic);

            Decimal sald = 0m;
            var receivedTotal = await _appServiceReceivableBills.GetReceivedTotal(IdAthletic);
            var paidTotal = await _appServiceReceivablePay.GetPaidTotal(IdAthletic);
            sald = (receivedTotal - paidTotal);

            List<DashGraficCashFlow> listGrafic = new List<DashGraficCashFlow>();
            if (days == 0) days = 60;
            for (int i = 0; i <= days; i++)
            {
                var date = DateTime.Now.AddDays(i);
                var receivableDay = await _appServiceReceivableBills.GetMyReceivableBillsperDate(IdAthletic, date);
                var paymentDay = await _appServiceReceivablePay.GetMyReceivablePaysPerDate(IdAthletic, date, null);
                CultureInfo culture = new CultureInfo("pt-BR");

                Decimal valueReceible = 0;
                Decimal valuePayment = 0;

                foreach (var rec in receivableDay)
                {
                    valueReceible += Convert.ToDecimal(rec.NetValue);
                    sald += Convert.ToDecimal(rec.NetValue);
                }

                foreach (var item in paymentDay)
                {
                    valuePayment += Convert.ToDecimal(item.TitleValue);
                    sald -= Convert.ToDecimal(item.TitleValue);
                }

                listGrafic.Add(new DashGraficCashFlow(date.ToString("dd"), valuePayment * (-1), valueReceible, sald, 0));

            }

            var dash = new DashViewModel(receivable.ToString("F2"), pay.ToString("F2"), associate, sales.TicketSale.ToString("F2"), sales.ProductSale.ToString("F2"), sales.PlanSale.ToString("F2"), sales.OutherSale.ToString("F2"), listGrafic);
            return dash;
        }

        public async Task<DashOwnerViewModel> GetDashOnwer(string IdAthletic, string IdUser)
        {
            var receivedTotal = await _appServiceReceivableBills.GetReceivedTotal(IdAthletic);
            var paidTotal = await _appServiceReceivablePay.GetPaidTotal(IdAthletic);
            var currentCashTotal = (receivedTotal - paidTotal);
            var events = await _appServiceEvent.GetEventDash(IdAthletic);
            var eventModel = new List<DashEventOwnerVidewModel>();
            foreach (var item in events)
            {
                int sold = 0;
                int available = 0;
                foreach (var area in item.EventArea)
                {
                    foreach (var lot in area.PassaportTicketLot)
                    {
                        sold += (int)lot.AmmountSold;
                        available += (lot.AmmountLimit - (int)lot.AmmountSold);
                    }
                }
                eventModel.Add(new DashEventOwnerVidewModel(item.Title, item.Local, item.EventType, sold, available));
            }

            var dashTrasaction = new List<DashTransactionOwnerViewModel>();

            foreach (var bill in await _appServiceReceivableBills.DashBills(IdAthletic))
            {
                dashTrasaction.Add(new DashTransactionOwnerViewModel(bill.DateTransaction, bill.Title, MovementType.INPUT, bill.Value));
            }

            foreach (var bill in await _appServiceReceivablePay.DashBills(IdAthletic))
            {
                dashTrasaction.Add(new DashTransactionOwnerViewModel(bill.DateTransaction, bill.Title, MovementType.OUTPUT, bill.Value));
            }

            var ordem = dashTrasaction.OrderByDescending(x => x.DateTransaction).ToList();

            var dash = new DashOwnerViewModel(currentCashTotal.ToString("F2"), await _appServiceUser.AmountAssociate(IdAthletic), await _appServiceUser.AmountNonAssociate(IdAthletic), eventModel, ordem);
            return dash;
        }

        public async Task<string> ExportTransaction(string idAthletic)
        {

            var dashTrasaction = new List<DashTransactionOwnerViewModel>();

            foreach (var bill in await _appServiceReceivableBills.DashBills(idAthletic))
            {
                dashTrasaction.Add(new DashTransactionOwnerViewModel(bill.DateTransaction, bill.Title, MovementType.INPUT, bill.Value));
            }

            foreach (var bill in await _appServiceReceivablePay.DashBills(idAthletic))
            {
                dashTrasaction.Add(new DashTransactionOwnerViewModel(bill.DateTransaction, bill.Title, MovementType.OUTPUT, bill.Value));
            }

            var ordem = dashTrasaction.OrderByDescending(x => x.DateTransaction).ToList();

            return await _appServiceExportFile.ExportExcelJson(ordem);

        }

        public async Task<DashViewModel> GetDash(string IdAthletic, string idCostCenter, int days)
        {
            var receivable = await _appServiceReceivableBills.GetMyReceivableMonth(IdAthletic, idCostCenter);
            var pay = await _appServiceReceivablePay.GetMyReceivableMonth(IdAthletic, idCostCenter);
            var associate = await _appServiceRequestAssociate.AmountAssociative(IdAthletic);
            var sales = await _appServiceOrder.DaySale(IdAthletic);

            Decimal sald = 0m;
            var receivedTotal = await _appServiceReceivableBills.GetReceivedTotal(IdAthletic);
            var paidTotal = await _appServiceReceivablePay.GetPaidTotal(IdAthletic);
            sald = (receivedTotal - paidTotal);

            List<DashGraficCashFlow> listGrafic = new List<DashGraficCashFlow>();
            if (days == 0) days = 60;
            for (int i = 0; i <= days; i++)
            {
                var date = DateTime.Now.AddDays(i);
                var receivableDay = await _appServiceReceivableBills.GetMyReceivableBillsperDate(IdAthletic, date, date, idCostCenter);
                var paymentDay = await _appServiceReceivablePay.GetMyReceivablePaysPerDate(IdAthletic, date, date, idCostCenter);
                CultureInfo culture = new CultureInfo("pt-BR");

                Decimal valueReceible = 0;
                Decimal valuePayment = 0;

                foreach (var rec in receivableDay)
                {
                    valueReceible += Convert.ToDecimal(rec.NetValue);
                    sald += Convert.ToDecimal(rec.NetValue);
                }

                foreach (var item in paymentDay)
                {
                    valuePayment += Convert.ToDecimal(item.TitleValue);
                    sald -= Convert.ToDecimal(item.TitleValue);
                }

                listGrafic.Add(new DashGraficCashFlow(date.ToString("dd"), valuePayment * (-1), valueReceible, sald, 0));

            }

            var dash = new DashViewModel(receivable.ToString("F2"), pay.ToString("F2"), associate, sales.TicketSale.ToString("F2"), sales.ProductSale.ToString("F2"), sales.PlanSale.ToString("F2"), sales.OutherSale.ToString("F2"), listGrafic);
            return dash;
        }

        public async Task<DashViewModel> GetDashDate(string IdAthletic, string IdUser, DateTime date, DateTime DateEnd, string idCenterCost = null)
        {
            // var receivable = await _appServiceReceivableBills.GetMyReceivableBillsperDate(IdAthletic, date, DateEnd, idCenterCost);
            // var pay = await _appServiceReceivablePay.GetMyReceivablePaysPerDate(IdAthletic, date, DateEnd, idCenterCost);
            var associate = await _appServiceRequestAssociate.AmountAssociative(IdAthletic);
            var sales = await _appServiceOrder.DaySale(IdAthletic);

            var recs = 0m;
            var pays = 0m;

            Decimal sald = 0m;
            var receivedTotal = await _appServiceReceivableBills.GetReceivedTotal(IdAthletic);
            var paidTotal = await _appServiceReceivablePay.GetPaidTotal(IdAthletic);
            sald = (receivedTotal - paidTotal);

            List<DashGraficCashFlow> listGrafic = new List<DashGraficCashFlow>();

            var quantityDate = DateEnd.Subtract(date).TotalDays;
            for (int i = 0; i <= quantityDate; i++)
            {
                var receivableDay = await _appServiceReceivableBills.GetMyReceivableBillsperDate(IdAthletic, date.AddDays(i));
                var paymentDay = await _appServiceReceivablePay.GetMyReceivablePaysPerDate(IdAthletic, date.AddDays(i), null, idCenterCost);

                CultureInfo culture = new CultureInfo("pt-BR");

                Decimal valueReceible = 0;
                Decimal valuePayment = 0;

                foreach (var rec in receivableDay)
                {
                    valueReceible += Convert.ToDecimal(rec.NetValue);
                    sald += Convert.ToDecimal(rec.NetValue);
                    if (rec.Status != "Recebido")
                        recs += valueReceible;
                }

                foreach (var item in paymentDay)
                {
                    valuePayment += Convert.ToDecimal(item.TitleValue);
                    sald -= Convert.ToDecimal(item.TitleValue);
                    if (item.Status != "Pago")
                        pays += valuePayment;
                }

                listGrafic.Add(new DashGraficCashFlow(date.AddDays(i).ToString("dd"), valuePayment * (-1), valueReceible, sald, 0));
            }


            var dash = new DashViewModel(recs.ToString("F2"), pays.ToString("F2"), associate, sales.TicketSale.ToString("F2"), sales.ProductSale.ToString("F2"), sales.PlanSale.ToString("F2"), sales.OutherSale.ToString("F2"), listGrafic);
            return dash;

        }

        public async Task<DashFinancialViewModel> GetDashFinancial(string IdAthletic)
        {
            //hoje
            var toReceiveToday = await _appServiceReceivableBills.GetToReceiveToday(IdAthletic);
            var toReceiveDatleticaToday = await _appServiceReceivableBills.GetToReceiveDatleticaToday(IdAthletic);
            var toPayToday = await _appServiceReceivablePay.GetToPayToday(IdAthletic);
            var receivedToday = await _appServiceReceivableBills.GetReceivedToday(IdAthletic);
            var paidToday = await _appServiceReceivablePay.GetPaidToday(IdAthletic);
            var currentCashToday = (receivedToday - paidToday);

            //fluxo de caixa geral
            var currentCashPrevision = await _appServiceReceivableBills.GetCashPrevisionAthletic(IdAthletic);
            var currentCashPrevision30 = currentCashPrevision.ValueThirtyDays;
            var currentCashPrevision60 = currentCashPrevision.ValueSixtyDays;
            // var toReceiveTotal = await _appServiceReceivableBills.GetMyAccountsReceive(IdAthletic);
            // var toReceiveDatleticaPay = await _appServiceReceivableBills.GetMyAccountsReceiveDatleticaPay(IdAthletic);
            var toReceiveDetached = await _appServiceReceivableBills.GetMyAccountsReceiveDetached(IdAthletic);
            var toPayTotal = await _appServiceReceivablePay.GetToPayAthletic(IdAthletic);
            var receivedTotal = await _appServiceReceivableBills.GetReceivedTotal(IdAthletic);
            var paidTotal = await _appServiceReceivablePay.GetPaidTotal(IdAthletic);
            var currentCashTotal = (receivedTotal - paidTotal);

            //datlética
            var transfers = await _appServiceOrder.GetTransferPaymentAthletic(IdAthletic);

            //fluxo de caixa geral também
            var toReceiveTotal = transfers.LockedValue + toReceiveDetached;

            return new DashFinancialViewModel(toReceiveToday.ToString("F2"), toReceiveDatleticaToday.ToString("F2"), toPayToday.ToString("F2"),
            currentCashToday.ToString("F2"), currentCashPrevision30.ToString("F2"), currentCashPrevision60.ToString("F2"),
            toReceiveTotal.ToString("F2"), transfers.LockedValue.ToString("F2"), toReceiveDetached.ToString("F2"), toPayTotal.ToString("F2"), currentCashTotal.ToString("F2"),
            transfers.ReleasedValue.ToString("F2"), transfers.LockedValue.ToString("F2"), transfers.TransferedValue.ToString("F2"));
        }

        public async Task<string> ExportDashFinancial(string idAthletic)
        {
            var export = new List<ExportFinancialPanelViewModel>();
            var athletics = await _unitOfWork.IRepositoryAthletic.GetByAsync(x => x.RecordSituation.Equals(RecordSituation.ACTIVE));
            foreach (var athletic in athletics)
            {
                var toReceiveToday = await _appServiceReceivableBills.GetToReceiveToday(athletic.Id);
                var toReceiveDatleticaToday = await _appServiceReceivableBills.GetToReceiveDatleticaToday(athletic.Id);
                var toPayToday = await _appServiceReceivablePay.GetToPayToday(athletic.Id);
                var receivedToday = await _appServiceReceivableBills.GetReceivedToday(athletic.Id);
                var paidToday = await _appServiceReceivablePay.GetPaidToday(athletic.Id);
                var currentCashToday = (receivedToday - paidToday);

                //fluxo de caixa geral
                var currentCashPrevision = await _appServiceReceivableBills.GetCashPrevisionAthletic(athletic.Id);
                var currentCashPrevision30 = currentCashPrevision.ValueThirtyDays;
                var currentCashPrevision60 = currentCashPrevision.ValueSixtyDays;
                var toReceiveDetached = await _appServiceReceivableBills.GetMyAccountsReceiveDetached(athletic.Id);
                var toPayTotal = await _appServiceReceivablePay.GetToPayAthletic(athletic.Id);
                var receivedTotal = await _appServiceReceivableBills.GetReceivedTotal(athletic.Id);
                var paidTotal = await _appServiceReceivablePay.GetPaidTotal(athletic.Id);
                var currentCashTotal = (receivedTotal - paidTotal);

                //datlética
                var transfers = await _appServiceOrder.GetTransferPaymentAthletic(athletic.Id);

                //fluxo de caixa geral também
                var toReceiveTotal = transfers.LockedValue + toReceiveDetached;

                export.Add(new ExportFinancialPanelViewModel(athletic.CompanyName, toReceiveToday.ToString("F2"), toReceiveDatleticaToday.ToString("F2"), toPayToday.ToString("F2"), currentCashToday.ToString("F2"),
                currentCashTotal.ToString("F2"), currentCashPrevision30.ToString("F2"), currentCashPrevision60.ToString("F2"), toReceiveTotal.ToString("F2"), transfers.LockedValue.ToString("F2"),
                toReceiveDetached.ToString("F2"), toPayTotal.ToString("F2"), transfers.TransferedValue.ToString("F2"), transfers.ReleasedValue.ToString("F2"), transfers.LockedValue.ToString("F2")));
            }

            return await _appServiceExportFile.ExportExcelJson(export);

        }

        /*public async Task<ExtractViewModel> GetExtract(string idAthletic, string idBank)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            List<ListReceivableAndPaysViewModel> listReceivableAndPays = new List<ListReceivableAndPaysViewModel>();
            Func<IQueryable<ReceivablePay>, IIncludableQueryable<ReceivablePay, object>> includesPay = t => t.Include(a => a.PaymentMethod);
            Func<IQueryable<ReceivableBills>, IIncludableQueryable<ReceivableBills, object>> includesReceivable = t => t.Include(a => a.PaymentMethod);

            IList<ReceivablePay> listPay = new List<ReceivablePay>();
            IList<ReceivableBills> listReceivable = new List<ReceivableBills>();
            if (idBank != null)
            {
                listPay = await _unitOfWork.IRepositoryReceivablePay.GetByOrderAsync
                    (x => x.IdAthletic.Equals(idAthletic) && x.IdBankAccount.Equals(idBank) && x.DateLow != null && x.DateLow != DateTime.MinValue && x.RecordSituation == RecordSituation.ACTIVE, x => x.DateLow, false, includesPay);

                listReceivable = await _unitOfWork.IRepositoryReceivableBills.GetByOrderAsync
                    (x => x.IdAthletic.Equals(idAthletic) && x.IdBankAccount.Equals(idBank) && (x.DateReceived != DateTime.MinValue && x.DateReceived != null) && x.RecordSituation == RecordSituation.ACTIVE, x => x.DateReceived, false, includesReceivable);
            }
            else
            {
                listPay = await _unitOfWork.IRepositoryReceivablePay.GetByOrderAsync
                    (x => x.IdAthletic.Equals(idAthletic) && x.DateLow != null && x.DateLow != DateTime.MinValue && x.RecordSituation == RecordSituation.ACTIVE, x => x.DateLow, false, includesPay);
                listReceivable = await _unitOfWork.IRepositoryReceivableBills.GetByOrderAsync
                    (x => x.IdAthletic.Equals(idAthletic) && x.DateReceived.HasValue && x.RecordSituation == RecordSituation.ACTIVE, x => x.DateReceived, false, includesReceivable);
            }
            decimal sumPay = 0;
            decimal sumReceivable = 0;
            string status = "Pendente";
            string buyerOrReceiver = "";
            var itemTitle = "";
            foreach (var item in listPay)
            {
                var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(item.IdUserWriterOff));
                var userName = "";
                if (user != null) userName = user.FullName;
                status = "Pago";
                if (item.Provider != null && item.Provider != "")
                {
                    buyerOrReceiver = item.Provider;
                }
                listReceivableAndPays.Add(new ListReceivableAndPaysViewModel(item.Title, "Conta a pagar", item.TitleValue.ToString("F2"), item.TitleValue, item.DateExpiration, null, userName, string.Empty, 0, item.IdBankAccount, status, buyerOrReceiver));
                sumPay += item.TitleValue;
            }

            foreach (var item in listReceivable)
            {
                if (item.IdPurchaseOrder != null)
                {
                    var order = await _unitOfWork.IRepositoryPurchaseOrder.GetByIdAsync(x => x.Id.Equals(item.IdPurchaseOrder));
                    if (order.FullName != null && order.FullName != "")
                    {
                        buyerOrReceiver = order.FullName;
                    }
                    
                    ProductStock productStock = null;
                    PlanAssociate planAssociate = null;
                    if (!string.IsNullOrEmpty(item.PurchaseOrder.PurchaseOrderItem.ElementAt(0).IdProductStock))
                    {
                        Func<IQueryable<ProductStock>, IIncludableQueryable<ProductStock, object>> includesProductStock = t => t.Include(a => a.Product);
                        productStock = await _unitOfWork.IRepositoryProductStock.GetByIdAsync(x => x.Id.Equals(item.PurchaseOrder.PurchaseOrderItem.ElementAt(0).IdProductStock), includesProductStock);
                        itemTitle = ", " + productStock.Product.Title;
                    }
                    else if (!string.IsNullOrEmpty(item.PurchaseOrder.PurchaseOrderItem.ElementAt(0).IdPlanAssociate))
                    {
                        planAssociate = await _unitOfWork.IRepositoryPlanAssociate.GetByIdAsync(x => x.Id.Equals(item.PurchaseOrder.PurchaseOrderItem.ElementAt(0).IdPlanAssociate));
                        itemTitle = ", " + planAssociate.Title;
                    }
                    else if (!string.IsNullOrEmpty(item.PurchaseOrder.PurchaseOrderItem.ElementAt(0).IdPassaportTicketLot))
                    {
                        itemTitle = ", " + item.PurchaseOrder.PurchaseOrderItem.ElementAt(0).PassaportTicketLot.Event.Title;
                    }
                }
                var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(item.IdUserWriterOff));
                var userName = "";
                if (user != null) userName = user.FullName;
                status = "Recebido";
                listReceivableAndPays.Add(new ListReceivableAndPaysViewModel(item.Title + itemTitle, "Conta a receber", item.NetValue.ToString("F2"), item.NetValue, item.DateReceived, item.ReceiveDate, userName, string.Empty, 1, item.IdBankAccount, status, buyerOrReceiver));
                sumReceivable += item.NetValue;
            }

            decimal sumListPay = 0;
            decimal sumListReceivable = 0;
            decimal sumTotal = 0;
            var ordenada = listReceivableAndPays.OrderBy(m => m.ExpirationOrReceiveDate);
            foreach (var item in ordenada)
            {
                if (item.Type == 0)
                {
                    sumListPay += item.ValueDecimal;
                }
                else
                {
                    sumListReceivable += item.ValueDecimal;
                }
                item.Balance = (sumListReceivable - sumListPay).ToString("F2");
                sumTotal += (sumListReceivable - sumListPay);
            }

            return new ExtractViewModel(sumTotal.ToString("F2"), ordenada.ToList());
        }*/

        /*public async Task<ExtractViewModel> GetMyExtractDates(string IdAthletic, DateTime dateStart, DateTime dateEnd, string idBank, int days, bool pastYears)
        {
            var extract = await FilterExtractPerBank(IdAthletic, idBank, days, pastYears);
            var list = extract.ReceivableAndPays;

            IEnumerable<ListReceivableAndPaysViewModel> listFiltered;
            if (dateStart == DateTime.MinValue)
            {
                listFiltered = list.Where(x => x.ReceiveOrDateLow.Value.Date <= dateEnd.Date);
            }
            else if (dateEnd == DateTime.MinValue)
            {
                listFiltered = list.Where(x => x.ReceiveOrDateLow.Value.Date >= dateStart.Date);
            }
            else
            {

                listFiltered = list.Where(x => x.ReceiveOrDateLow.Value.Date >= dateStart.Date
                                               && x.ReceiveOrDateLow.Value.Date <= dateEnd.Date);
            }
            extract.ReceivableAndPays = listFiltered.ToList();

            return extract;
        }
        // filtro geral extrato
        public async Task<ExtractViewModel> FilterExtract(string idAthletic, string value, string idBank, int days, bool pastYears)
        {
            var extract = await FilterExtractPerBank(idAthletic, idBank, days, pastYears);
            var list = extract.ReceivableAndPays;

            var listFiltered = list.Where(x => x.Title.Contains(value, System.StringComparison.CurrentCultureIgnoreCase)
                                          || x.Value.Contains(value, System.StringComparison.CurrentCultureIgnoreCase)
                                          || x.Provider.Contains(value, System.StringComparison.CurrentCultureIgnoreCase)
                                          || x.TypeString.Contains(value, System.StringComparison.CurrentCultureIgnoreCase)
                                          || x.Status.Contains(value, System.StringComparison.CurrentCultureIgnoreCase)
                                          || x.UserWriterOff.Contains(value, System.StringComparison.CurrentCultureIgnoreCase));
            extract.ReceivableAndPays = listFiltered.ToList();

            return extract;
        }

        public async Task<ExtractViewModel> FilterExtractPerBank(string idAthletic, string idBank, int days, bool pastYears)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var date = DateTime.Now.ToBrasilia();
            List<ListReceivableAndPaysViewModel> listReceivableAndPays = new List<ListReceivableAndPaysViewModel>();
            Func<IQueryable<ReceivablePay>, IIncludableQueryable<ReceivablePay, object>> includesPay = t => t.Include(a => a.PaymentMethod);
            Func<IQueryable<ReceivableBills>, IIncludableQueryable<ReceivableBills, object>> includesReceivable = t => t.Include(a => a.PaymentMethod);

            IList<ReceivablePay> listPay = new List<ReceivablePay>();
            IList<ReceivableBills> listReceivable = new List<ReceivableBills>();
            if (idBank != null)
            {
                if (pastYears)
                {
                    listPay = await _unitOfWork.IRepositoryReceivablePay.GetByOrderAsync(x => x.IdAthletic.Equals(idAthletic) && x.IdBankAccount.Equals(idBank) && x.DateLow != null && x.DateLow != DateTime.MinValue && x.RecordSituation == RecordSituation.ACTIVE && x.DateLow.Date <= date.AddDays(-365), x => x.DateLow, true, includesPay);
                    listReceivable = await _unitOfWork.IRepositoryReceivableBills.GetByOrderAsync(x => x.IdAthletic.Equals(idAthletic) && x.IdBankAccount.Equals(idBank) && (x.DateReceived != DateTime.MinValue && x.DateReceived != null) && x.RecordSituation == RecordSituation.ACTIVE && x.DateReceived <= date.AddDays(-365), x => x.DateReceived, true, includesReceivable);
                }
                else
                {
                    listPay = await _unitOfWork.IRepositoryReceivablePay.GetByOrderAsync(x => x.IdAthletic.Equals(idAthletic) && x.IdBankAccount.Equals(idBank) && x.DateLow != null && x.DateLow != DateTime.MinValue && x.RecordSituation == RecordSituation.ACTIVE && x.DateLow.Date >= date.AddDays(-days), x => x.DateLow, true, includesPay);
                    listReceivable = await _unitOfWork.IRepositoryReceivableBills.GetByOrderAsync(x => x.IdAthletic.Equals(idAthletic) && x.IdBankAccount.Equals(idBank) && (x.DateReceived != DateTime.MinValue && x.DateReceived != null) && x.RecordSituation == RecordSituation.ACTIVE && x.DateReceived >= date.AddDays(-days), x => x.DateReceived, true, includesReceivable);
                }
            }
            else
            {
                if (pastYears)
                {
                    listPay = await _unitOfWork.IRepositoryReceivablePay.GetByOrderAsync(x => x.IdAthletic.Equals(idAthletic) && x.DateLow != null && x.DateLow != DateTime.MinValue && x.RecordSituation == RecordSituation.ACTIVE && x.DateLow.Date <= date.AddDays(-365), x => x.DateLow, true, includesPay);
                    listReceivable = await _unitOfWork.IRepositoryReceivableBills.GetByOrderAsync(x => x.IdAthletic.Equals(idAthletic) && (x.DateReceived != DateTime.MinValue && x.DateReceived != null) && x.RecordSituation == RecordSituation.ACTIVE && x.DateReceived <= date.AddDays(-365), x => x.DateReceived, true, includesReceivable);
                }
                else
                {
                    listPay = await _unitOfWork.IRepositoryReceivablePay.GetByOrderAsync(x => x.IdAthletic.Equals(idAthletic) && x.DateLow != null && x.DateLow != DateTime.MinValue && x.RecordSituation == RecordSituation.ACTIVE && x.DateLow.Date >= date.AddDays(-days), x => x.DateLow, true, includesPay);
                    listReceivable = await _unitOfWork.IRepositoryReceivableBills.GetByOrderAsync(x => x.IdAthletic.Equals(idAthletic) && (x.DateReceived != DateTime.MinValue && x.DateReceived != null) && x.RecordSituation == RecordSituation.ACTIVE && x.DateReceived >= date.AddDays(-days), x => x.DateReceived, true, includesReceivable);
                }
            }
            decimal sumPay = 0;
            decimal sumReceivable = 0;
            string status = "Pendente";
            string buyerOrReceiver = "";
            var itemTitle = "";
            foreach (var item in listPay)
            {
                var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(item.IdUserWriterOff));
                var userName = "";
                if (user != null) userName = user.FullName;
                status = "Pago";
                if (item.Provider != null && item.Provider != "")
                {
                    buyerOrReceiver = item.Provider;
                }
                listReceivableAndPays.Add(new ListReceivableAndPaysViewModel(item.Title, "Conta a pagar", item.TitleValue.ToString("F2"), item.TitleValue, item.DateLow, userName, string.Empty, 0, item.IdBankAccount, status, buyerOrReceiver));
                sumPay += item.TitleValue;
            }

            foreach (var item in listReceivable)
            {
                if (item.IdPurchaseOrder != null)
                {
                    //var order = await _unitOfWork.IRepositoryPurchaseOrder.GetByIdAsync(x => x.Id.Equals(item.IdPurchaseOrder));
                    Func<IQueryable<ReceivableBills>, IIncludableQueryable<ReceivableBills, object>> receivable = x => x.Include(a => a.PaymentMethod).Include(b => b.CostCenter).Include(c => c.PurchaseOrder).ThenInclude(d => d.PurchaseOrderItem).ThenInclude(e => e.PassaportTicketLot).ThenInclude(f => f.Event);
                    var order = await _unitOfWork.IRepositoryReceivableBills.GetByIdAsync(x => x.IdPurchaseOrder.Equals(item.IdPurchaseOrder), receivable);
                    if (order.PurchaseOrder.FullName != null && order.PurchaseOrder.FullName != "")
                    {
                        buyerOrReceiver = order.PurchaseOrder.FullName;
                    }

                    ProductStock productStock = null;
                    PlanAssociate planAssociate = null;
                    if (!string.IsNullOrEmpty(item.PurchaseOrder.PurchaseOrderItem.ElementAt(0).IdProductStock))
                    {
                        Func<IQueryable<ProductStock>, IIncludableQueryable<ProductStock, object>> includesProductStock = t => t.Include(a => a.Product);
                        productStock = await _unitOfWork.IRepositoryProductStock.GetByIdAsync(x => x.Id.Equals(item.PurchaseOrder.PurchaseOrderItem.ElementAt(0).IdProductStock), includesProductStock);
                        itemTitle = ", " + productStock.Product.Title;
                    }
                    else if (!string.IsNullOrEmpty(item.PurchaseOrder.PurchaseOrderItem.ElementAt(0).IdPlanAssociate))
                    {
                        planAssociate = await _unitOfWork.IRepositoryPlanAssociate.GetByIdAsync(x => x.Id.Equals(item.PurchaseOrder.PurchaseOrderItem.ElementAt(0).IdPlanAssociate));
                        itemTitle = ", " + planAssociate.Title;
                    }
                    else if (!string.IsNullOrEmpty(item.PurchaseOrder.PurchaseOrderItem.ElementAt(0).IdPassaportTicketLot))
                    {
                        itemTitle = ", " + item.PurchaseOrder.PurchaseOrderItem.ElementAt(0).PassaportTicketLot.Event.Title;
                    }
                }

                var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(item.IdUserWriterOff));
                var userName = "";
                if (user != null)
                {
                    userName = user.FullName;
                }
                else
                {
                    userName = "Baixa automática";
                }
                status = "Recebido";
                listReceivableAndPays.Add(new ListReceivableAndPaysViewModel(item.Title + itemTitle, "Conta a receber", item.NetValue.ToString("F2"), item.NetValue, item.DateReceived, userName, string.Empty, 1, item.IdBankAccount, status, buyerOrReceiver));
                sumReceivable += item.NetValue;
            }

            decimal sumListPay = 0;
            decimal sumListReceivable = 0;
            decimal sumTotal = 0;
            var ordenada = listReceivableAndPays.OrderBy(m => m.ReceiveOrDateLow);
            foreach (var item in ordenada)
            {
                if (item.Type == 0)
                {
                    sumListPay += item.ValueDecimal;
                }
                else
                {
                    sumListReceivable += item.ValueDecimal;
                }
                item.Balance = (sumListReceivable - sumListPay).ToString("F2");
                sumTotal += (sumListReceivable - sumListPay);
            }

            return new ExtractViewModel(sumTotal.ToString("F2"), ordenada.ToList());
        }*/
    }
}
