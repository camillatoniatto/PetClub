﻿using Datletica.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.UriAppService.Interfaces;
using PetClub.AppService.AppServices.UserAppService;
using PetClub.AppService.AppServices.UsersPartnersAppService;
using PetClub.AppService.ViewModels.Scheduler;
using PetClub.Configurations;
using PetClub.CrossCutting.Identity.Interfaces;
using PetClub.Domain.Entities;

namespace PetClub.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersPartnersController : MainController
    {
        private IUriAppService _uriAppService;
        private readonly IEmailSenderService _emailSender;
        private readonly IAppServiceUsersPartners _appServiceUsersPartners;
        private readonly IAppServiceUser _appServiceUser;

        public UsersPartnersController(INotifierAppService notifier, IUriAppService uriAppService, IEmailSenderService emailSender, IAppServiceUsersPartners appServiceUsersPartners, IAppServiceUser appServiceUser) : base(notifier)
        {
            _uriAppService = uriAppService;
            _emailSender = emailSender;
            _appServiceUsersPartners = appServiceUsersPartners;
            _appServiceUser = appServiceUser;
        }

        [HttpPost]
        [Route("get-user-cpf")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> CreateUsersPartners(string cpf, bool save)
        {
            try
            {
                var partner = GetUser();
                var user = await _appServiceUser.GetByCpf(cpf);
                if (save)
                {
                    var response = await _appServiceUsersPartners.CreateUsersPartners(user.Id, partner.Id);
                }
                return CustomResponse(user);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-users-partner")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> GetUsersPartners()
        {
            var user = GetUser();
            try
            {
                var response = await _appServiceUsersPartners.GetUsersPartners(user.Id);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-users-partner-details")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PARTNER)]
        public async Task<IActionResult> GetByIdUsersPartners(string idUsersPartners)
        {
            try
            {
                var response = await _appServiceUsersPartners.GetByIdUsersPartners(idUsersPartners);
                return CustomResponse(response);
            }
            catch
            {
                return CustomResponse();
            }
        }
    }
}
