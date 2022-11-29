using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PetClub.AppService.AppServices.CashFlowAppService;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.Domain.Entities;
using PetClub.Domain.Enum;
using PetClub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.AppService.ViewModels.PaymentMethod;

namespace PetClub.AppService.AppServices.PaymentMethodAppService
{
    public class AppServicePaymentMethod : IAppServicePaymentMethod
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotifierAppService _notifier;

        public AppServicePaymentMethod(IUnitOfWork unitOfWork, IMapper mapper, INotifierAppService notifier)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
        }

        public async Task AddPaymentMethod(CreatePaymentMethodViewModel model)
        {
            try
            {
                var method = await _unitOfWork.IRepositoryPaymentMethod.GetByAsync(x => x.Title.Equals(model.Title)
                                                                                && x.PaymentType == model.PaymentType
                                                                                && x.NumberInstallments == model.NumberInstallments
                                                                                && x.RecordSituation == RecordSituation.ACTIVE);
                if (method.Count > 0)
                {
                    _notifier.Handle(new NotificationMessage("payment", "Já existe essa forma de pagamento"));
                    throw new Exception("Já existe essa forma de pagamento");
                }

                await _unitOfWork.IRepositoryPaymentMethod.AddAsync(new PaymentMethod(model.Title, model.PaymentType, model.IsInstallment, model.NumberInstallments, model.AdminTax));
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {

            }
        }

        public async Task CreateAllPayments()
        {
            var payment = await _unitOfWork.IRepositoryPaymentMethod.GetByAsync(x => x.RecordSituation.Equals(RecordSituation.ACTIVE));
            if (payment.Count == 0)
            {
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CASH, false, 1, 0M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.PIX, false, 1, 4M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.DEBIT, false, 1, 6M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CREDIT_CARD, true, 1, 6M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CREDIT_CARD, true, 2, 6M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CREDIT_CARD, true, 3, 6M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CREDIT_CARD, true, 4, 6M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CREDIT_CARD, true, 5, 6M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CREDIT_CARD, true, 6, 6M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CREDIT_CARD, true, 7, 6M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CREDIT_CARD, true, 8, 6M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CREDIT_CARD, true, 9, 6M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CREDIT_CARD, true, 10, 6M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CREDIT_CARD, true, 11, 6M));
                await AddPaymentMethod(new CreatePaymentMethodViewModel("PetClub Pay", PaymentType.CREDIT_CARD, true, 12, 6M));
            }            
        }

        public async Task<GetPaymentMethodViewModel> UpdateAsync(UpdatePaymentMethodViewModel model)
        {
            var payment = await _unitOfWork.IRepositoryPaymentMethod.GetByIdAsync(x => x.Id.Equals(model.IdPaymentMethod));
            //if (payment.ExclusiveApp)
            //{
            //    _notifier.Handle(new NotificationMessage("numberInstallments", "Esse meio de pagamento é exclusivo para o aplicativo, não será possível ser alterado."));
            //    throw new Exception();
            //}

            payment.NumberInstallments = model.NumberInstallments;
            payment.PaymentType = model.PaymentType;
            payment.IsInstallment = model.IsInstallment;
            payment.Title = model.Title;
            await _unitOfWork.IRepositoryPaymentMethod.UpdateAsync(payment);
            await _unitOfWork.CommitAsync();

            var paymentType = GetPaymentType(payment.PaymentType, payment.NumberInstallments);
            return new GetPaymentMethodViewModel(payment.Id, payment.Title, paymentType, payment.IsInstallment, payment.NumberInstallments, payment.AdminTax, payment.DateCreation);
        }

        public async Task DeleteAsync(string Id)
        {
            var methods = await _unitOfWork.IRepositoryPaymentMethod.GetByIdAsync(x => x.Id.Equals(Id));
            //if (methods.ExclusiveApp)
            //{
            //    _notifier.Handle(new NotificationMessage("numberInstallments", "Esse meio de pagamento é exclusivo para o aplicativo, não será possível ser alterado alterado."));
            //    throw new Exception();
            //}
            methods.RecordSituation = RecordSituation.INACTIVE;
            await _unitOfWork.IRepositoryPaymentMethod.UpdateAsync(methods);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<GetPaymentMethodViewModel>> GetAllPaymentMethods()
        {
            var paymentMethods = await _unitOfWork.IRepositoryPaymentMethod.GetByOrderAsync(x => x.RecordSituation == RecordSituation.ACTIVE, x => x.NumberInstallments, false);

            var list = new List<GetPaymentMethodViewModel>();
            foreach (var payment in paymentMethods)
            {
                var paymentType = GetPaymentType(payment.PaymentType, payment.NumberInstallments);
                list.Add(new GetPaymentMethodViewModel(payment.Id, payment.Title, paymentType, payment.IsInstallment, payment.NumberInstallments, payment.AdminTax, payment.DateCreation));
            }
            return list.OrderBy(x => x.DateCreation).ToList();
        }        

        public async Task<GetPaymentMethodViewModel> GetByIdAsync(string Id)
        {
            var payment = await _unitOfWork.IRepositoryPaymentMethod.GetByIdAsync(x => x.Id.Equals(Id));
            var paymentType = GetPaymentType(payment.PaymentType, payment.NumberInstallments);
            return new GetPaymentMethodViewModel(payment.Id, payment.Title, paymentType, payment.IsInstallment, payment.NumberInstallments, payment.AdminTax, payment.DateCreation);
        }

        public async Task<List<SelectPaymentMethodViewModel>> GetNumberInstallmentsPerPaymentType(string idAthletic, PaymentType paymentMethodType)
        {
            var list = new List<SelectPaymentMethodViewModel>();
            var methods = await _unitOfWork.IRepositoryPaymentMethod.GetByOrderAsync(x => x.PaymentType == paymentMethodType && x.RecordSituation == RecordSituation.ACTIVE, x => x.NumberInstallments, false);
            foreach (var item in methods)
            {
                list.Add(new SelectPaymentMethodViewModel(item.Id, item.NumberInstallments.ToString()));
            }
            return list;
        }

        //public async Task<Dictionary<string, string>> GetAdminPaymentTax(decimal value, string idAthletic)
        //{
        //    double finalTax = 0;
        //    decimal valueFinal = 0;
        //    var taxAdmin = await GetExclusiveApp();
        //    var response = new Dictionary<string, string>();
        //    foreach (var creditCard in taxAdmin.Where(x => x.PaymentType.Equals(PaymentType.CREDIT_CARD)))
        //    {
        //        finalTax = Convert.ToDouble(creditCard.AdminTax) != 0 ? Convert.ToDouble(creditCard.AdminTax) : 6;
        //        valueFinal = value / (1 - (decimal)finalTax / 100);
        //        response.Add("Cartão de Crédito", valueFinal.ToString("F2"));
        //    }

        //    foreach (var pix in taxAdmin.Where(x => x.PaymentType.Equals(PaymentType.PIX)))
        //    {
        //        finalTax = Convert.ToDouble(pix.AdminTax) != 0 ? Convert.ToDouble(pix.AdminTax) : 4;
        //        valueFinal = value / (1 - (decimal)finalTax / 100);
        //        response.Add("Pix", valueFinal.ToString("F2"));
        //    }

        //    return response;
        //}

        public string GetPaymentType(PaymentType paymentType, int installments)
        {
            var result = "";
            switch (paymentType)
            {
                case PaymentType.CREDIT_CARD:
                    result = "Cartão de Crédito em "+installments+"X";
                    break;
                case PaymentType.CASH:
                    result = "Dinheiro";
                    break;
                case PaymentType.DEBIT:
                    result = "Débito";
                    break;
                case PaymentType.PIX:
                    result = "Pix";
                    break;
            }
            return result;
        }


    }
}
