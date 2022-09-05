﻿using PetClub.AppService.ViewModels.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.PaymentMethodAppService
{
    public interface IAppServicePaymentMethod
    {
        Task AddPaymentMethod(CreatePaymentMethodViewModel model);
        Task CreateAllPayments();
        Task DeleteAsync(string Id);
        Task<List<GetPaymentMethodViewModel>> GetAllPaymentMethods();
        Task<GetPaymentMethodViewModel> GetByIdAsync(string Id);
        Task<GetPaymentMethodViewModel> UpdateAsync(UpdatePaymentMethodViewModel model);
    }
}
