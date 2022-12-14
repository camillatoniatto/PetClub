using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.PetAppService;
using PetClub.AppService.AppServices.SchedulerAppService;
using PetClub.AppService.AppServices.UriAppService.Interfaces;
using PetClub.AppService.AppServices.UserAppService;
using PetClub.AppService.ViewModels.Pet;
using PetClub.AppService.ViewModels.Scheduler;
using PetClub.Configurations;
using PetClub.CrossCutting.Identity.Interfaces;

namespace PetClub.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class SchedulerController : MainController
    {
        private IUriAppService _uriAppService;
        private readonly IEmailSenderService _emailSender;
        private readonly IAppServiceScheduler _appServiceScheduler;

        public SchedulerController(INotifierAppService notifier, IUriAppService uriAppService, IEmailSenderService emailSender, IAppServiceScheduler appServiceScheduler) : base(notifier)
        {
            _uriAppService = uriAppService;
            _emailSender = emailSender;
            _appServiceScheduler = appServiceScheduler;
        }

        [HttpPost]
        [Route("create-scheduler")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> CreateScheduler(CreateSchedulerViewModel model)
        {
            var user = GetUser();
            try
            {
                model.IdPartner = user.Id;
                var response = await _appServiceScheduler.CreateScheduler(model);
                return CustomResponse(response);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-scheduler-partner")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> GetSchedulersByPartner()
        {
            var user = GetUser();
            try
            {
                var response = await _appServiceScheduler.GetSchedulersByPartner(user.Id);
                return CustomResponse(response);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-scheduler-pet")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> GetSchedulersByPet(string idPet)
        {
            try
            {
                var response = await _appServiceScheduler.GetSchedulersByPet(idPet);
                return CustomResponse(response);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-scheduler-byid")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> GetSchedulersById(string idScheduler)
        {
            try
            {
                var response = await _appServiceScheduler.GetSchedulersById(idScheduler);
                return CustomResponse(response);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("check-dates-scheduler")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> CheckQuantitySchedylers(DateTime startDate, DateTime endDate)
        {
            try
            {
                var check = await _appServiceScheduler.CheckQuantitySchedylers(startDate, endDate);
                return CustomResponse(check);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }

        [HttpPut]
        [Route("update-scheduler")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> UpdateScheduler(UpdateSchedulerViewModel model)
        {
            try
            {
                var user = GetUser();
                model.IdPartner = user.Id;
                await _appServiceScheduler.UpdateScheduler(model, user.Id);
                return CustomResponse();
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }        

        [HttpDelete]
        [Route("delete-scheduler")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> DeleteScheduler(string idScheduler)
        {
            try
            {
                await _appServiceScheduler.DeleteScheduler(idScheduler);
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
