using Datletica.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.PaymentMethodAppService;
using PetClub.AppService.AppServices.PurchaseOrderAppService;
using PetClub.AppService.AppServices.UriAppService.Interfaces;
using PetClub.AppService.AppServices.UserAppService;
using PetClub.AppService.ViewModels.PaymentMethod;
using PetClub.AppService.ViewModels.PurchaseOrder;
using PetClub.Configurations;
using PetClub.CrossCutting.Identity.Interfaces;

namespace PetClub.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PurchaseOrderController : MainController
    {
        private IAppServiceUser _appServiceUser;
        private IUriAppService _uriAppService;
        private readonly IEmailSenderService _emailSender;
        private readonly IAppServicePurchaseOrder _appServicePurchaseOrder;

        public PurchaseOrderController(INotifierAppService notifier, IUriAppService uriAppService, IEmailSenderService emailSender, IAppServicePurchaseOrder appServicePurchaseOrder) : base(notifier)
        {
            _uriAppService = uriAppService;
            _emailSender = emailSender;
            _appServicePurchaseOrder = appServicePurchaseOrder;
        }

        [HttpPost]
        [Route("create-order")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> CreatePurchaseOrder(CreatePurchaseOrderViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                var user = GetUser();
                var response = await _appServicePurchaseOrder.CreatePurchaseOrder(model, user.Id);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-order-byid")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> GetPurchaseOrderById(string idPurchaseOrder)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                var user = GetUser();
                var response = await _appServicePurchaseOrder.GetPurchaseOrderById(idPurchaseOrder);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-list-order-user")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> GetPurchaseOrdersUser(bool isApp)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                var user = GetUser();
                var response = await _appServicePurchaseOrder.GetPurchaseOrdersUser(user.Id, isApp);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpPut]
        [Route("update-order")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> UpdadePurchaseOrder(UpdatePurchaseOrderViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                //var user = GetUser();
                await _appServicePurchaseOrder.UpdadePurchaseOrder(model);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpPut]
        [Route("finish-order")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> ConcluedPurchaseOrder(string idPurchaseOrder, bool isPaid)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                //var user = GetUser();
                await _appServicePurchaseOrder.ConcluedPurchaseOrder(idPurchaseOrder, isPaid);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpDelete]
        [Route("delete-order")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> DeletePurchaseOrder(string idPurchaseOrder)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                //var user = GetUser();
                await _appServicePurchaseOrder.DeletePurchaseOrder(idPurchaseOrder);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }
    }
}
