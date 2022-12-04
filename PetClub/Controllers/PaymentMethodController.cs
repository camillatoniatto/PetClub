using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.PaymentMethodAppService;
using PetClub.AppService.AppServices.UriAppService.Interfaces;
using PetClub.AppService.AppServices.UserAppService;
using PetClub.AppService.ViewModels.PaymentMethod;
using PetClub.AppService.ViewModels.Pet;
using PetClub.Configurations;
using PetClub.CrossCutting.Identity.Interfaces;

namespace PetClub.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PaymentMethodController : MainController
    {
        private IAppServiceUser _appServiceUser;
        private IUriAppService _uriAppService;
        private readonly IEmailSenderService _emailSender;
        private readonly IAppServicePaymentMethod _appServicePaymentMethod;

        public PaymentMethodController(INotifierAppService notifier, IAppServiceUser appServiceUser, IUriAppService uriAppService, IEmailSenderService emailSender, IAppServicePaymentMethod appServicePaymentMethod) : base(notifier)
        {
            _appServiceUser = appServiceUser;
            _uriAppService = uriAppService;
            _emailSender = emailSender;
            _appServicePaymentMethod = appServicePaymentMethod;
        }

        [HttpPost]
        [Route("create-payment")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.ADMIN_SYSTEM)]
        public async Task<IActionResult> AddPaymentMethod(CreatePaymentMethodViewModel model)
        {
            try
            {
                await _appServicePaymentMethod.AddPaymentMethod(model);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpPost]
        [Route("create-all-payment")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.ADMIN_SYSTEM)]
        public async Task<IActionResult> CreateAllPayments()
        {
            try
            {
                await _appServicePaymentMethod.CreateAllPayments();
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-all-payments")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> GetAllPaymentMethods()
        {
            try
            {
                var response = await _appServicePaymentMethod.GetAllPaymentMethods();
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-payment-by-id")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> GetByIdAsync(string Id)
        {
            try
            {
                var response = await _appServicePaymentMethod.GetByIdAsync(Id);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpPut]
        [Route("update-payment")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> UpdateAsync(UpdatePaymentMethodViewModel model)
        {
            try
            {
                var response = await _appServicePaymentMethod.UpdateAsync(model);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpDelete]
        [Route("delete-payment")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _appServicePaymentMethod.DeleteAsync(id);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }
    }
}
