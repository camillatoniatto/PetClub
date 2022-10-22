using Datletica.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetClub.AppService.AppServices.CashFlowAppService;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.PetAppService;
using PetClub.AppService.AppServices.UriAppService.Interfaces;
using PetClub.AppService.AppServices.UserAppService;
using PetClub.AppService.ViewModels.CashFlow;
using PetClub.AppService.ViewModels.Pet;
using PetClub.Configurations;
using PetClub.CrossCutting.Identity.Interfaces;

namespace PetClub.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class CashFlowController : MainController
    {
        private IUriAppService _uriAppService;
        private readonly IEmailSenderService _emailSender;
        private readonly IAppServiceCashFlow _appServiceCashFlow;

        public CashFlowController(INotifierAppService notifier,IUriAppService uriAppService, IEmailSenderService emailSender, IAppServiceCashFlow appServiceCashFlow) : base(notifier)
        {
            _uriAppService = uriAppService;
            _emailSender = emailSender;
            _appServiceCashFlow = appServiceCashFlow;
        }

        [HttpPost]
        [Route("create-bill")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> CreateReceivableBill(CreateCashFlowViewModel model)
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                model.IdUserCreate = user.Id;
                await _appServiceCashFlow.CreateReceivableBill(model, user.Id);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-cashflow")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> GetCashFlow()
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                await _appServiceCashFlow.GetCashFlow(user.Id);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpPut]
        [Route("write-off-bill")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> WriteOff(string idCashFlow)
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                await _appServiceCashFlow.WriteOff(idCashFlow, user.Id);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpPut]
        [Route("update-bill")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> UpdateAsync(UpdateCashFlowViewModel model)
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                await _appServiceCashFlow.UpdateAsync(model, user.Id);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpDelete]
        [Route("delete-bill")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> DeleteBill(string idCashFlow)
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                await _appServiceCashFlow.DeleteBill(idCashFlow, user.Id);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }
    }
}
