﻿using Datletica.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.PetAppService;
using PetClub.AppService.AppServices.ServiceAppService;
using PetClub.AppService.AppServices.UriAppService.Interfaces;
using PetClub.AppService.AppServices.UserAppService;
using PetClub.AppService.ViewModels.Pet;
using PetClub.AppService.ViewModels.Service;
using PetClub.Configurations;
using PetClub.CrossCutting.Identity.Interfaces;

namespace PetClub.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceController : MainController
    {
        private IAppServiceUser _appServiceUser;
        private IUriAppService _uriAppService;
        private readonly IEmailSenderService _emailSender;
        private readonly IAppServiceService _appServiceService;

        public ServiceController(INotifierAppService notifier, IAppServiceUser appServiceUser, IUriAppService uriAppService, IEmailSenderService emailSender, IAppServiceService appServiceService) : base(notifier)
        {
            _appServiceUser = appServiceUser;
            _uriAppService = uriAppService;
            _emailSender = emailSender;
            _appServiceService = appServiceService;
        }

        [HttpPost]
        [Route("create-service")]
        [ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> CreateService(CreateServiceViewModel model)
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                var response = await _appServiceService.CreateService(model, user.Id);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-service-by-id")]
        [ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> GetServiceById(string idService)
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                var response = await _appServiceService.GetServiceById(idService);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-service-by-idUser")]
        [ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> GetPetsUser()
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                var response = await _appServiceService.GetServiceUser(user.Id);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        /*[HttpGet]
        [Route("get-all-services")]
        [ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> GetAllPets()
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                var response = await _appServiceService.GetAllServices();
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }*/

        [HttpPut]
        [Route("update-service")]
        [ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> UpdateService(UpdateServiceViewModel model)
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                await _appServiceService.UpdateService(model);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpDelete]
        [Route("delete-service")]
        [ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> DeleteService(string idService)
        {
            var user = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            try
            {
                await _appServiceService.DeleteService(idService);
                return CustomResponse();
            }
            catch
            {
                return CustomResponse();
            }
        }
    }
}
