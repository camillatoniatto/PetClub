using Datletica.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.PetAppService;
using PetClub.AppService.AppServices.ServiceRefreshTokenAppService;
using PetClub.AppService.AppServices.UriAppService.Interfaces;
using PetClub.AppService.AppServices.UserAppService;
using PetClub.AppService.ViewModels.Pet;
using PetClub.Configurations;
using PetClub.CrossCutting.Identity.Interfaces;
using PetClub.CrossCutting.Identity.Models;

namespace PetClub.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class PetController : MainController
    {
        private IAppServiceUser _appServiceUser;
        private IUriAppService _uriAppService;
        private readonly IEmailSenderService _emailSender;
        private readonly IAppServicePet _appServicePet;

        public PetController(INotifierAppService notifier, IAppServiceUser appServiceUser, IUriAppService uriAppService, IEmailSenderService emailSender, IAppServicePet appServicePet) : base(notifier)
        {
            _appServiceUser = appServiceUser;
            _uriAppService = uriAppService;
            _emailSender = emailSender;
            _appServicePet = appServicePet;
        }

        [HttpPost]
        [Route("create-pet")]
        [ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> CreatePet(CreatePetViewModel model)
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                var response = await _appServicePet.CreatePet(model, user.Id);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-pet-by-id")]
        [ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> GetPetById(string idPet)
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                var response = await _appServicePet.GetPetById(idPet);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-pet-by-idUser")]
        [ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> GetPetsUser()
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                var response = await _appServicePet.GetPetsUser(user.Id);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-all-pets")]
        [ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> GetAllPets()
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                var response = await _appServicePet.GetAllPets();
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpPut]
        [Route("update-pet")]
        [ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> UpdatePet(UpdatePetViewModel model)
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                await _appServicePet.UpdatePet(model);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpDelete]
        [Route("delete-pet")]
        [ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> DeletePet(string idPet)
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                await _appServicePet.DeletePet(idPet);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }
    }
}
