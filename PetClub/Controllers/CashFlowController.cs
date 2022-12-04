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
            try
            {
                model.IdUserCreate = user.Id;
                await _appServiceCashFlow.CreateReceivableBill(model, user.Id);
                return CustomResponse();
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
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
            try
            {
                var cashflow = await _appServiceCashFlow.GetCashFlow(user.Id);
                return CustomResponse(cashflow);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-cashflow-admin")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> GetCashFlowAdmin()
        {
            var user = GetUser();
            try
            {
                var cashflow = await _appServiceCashFlow.GetCashFlowAdmin();
                return CustomResponse(cashflow);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-resume-cashflow")]
        [AllowAnonymous]
        public async Task<IActionResult> ResumeCashFlow()
        {
            var user = GetUser();
            try
            {
                var cashflow = await _appServiceCashFlow.ResumeCashFlow(user.Id);
                return CustomResponse(cashflow);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
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
            try
            {
                await _appServiceCashFlow.WriteOff(idCashFlow, user.Id);
                return CustomResponse();
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
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
            try
            {
                await _appServiceCashFlow.UpdateAsync(model, user.Id);
                return CustomResponse();
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }

        [HttpDelete]
        [Route("delete-bill")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> DeleteBill(string idCashFlow)
        {
            try
            {
                var user = GetUser();

                await _appServiceCashFlow.DeleteBill(idCashFlow, user.Id);
                return CustomResponse();
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }
    }
}
