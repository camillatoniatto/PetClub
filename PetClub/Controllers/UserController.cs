using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.UriAppService.Interfaces;
using PetClub.AppService.AppServices.UserAppService;
using PetClub.AppService.AppServices.ServiceRefreshTokenAppService;
using PetClub.CrossCutting.Identity.Interfaces;
using PetClub.CrossCutting.Identity.Models;
using PetClub.AppService.ViewModels.User;
using PetClub.Configurations;
using System.Security.Claims;

namespace PetClub.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : MainController
    {
        private IAppServiceUser _appServiceUser;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IAppServiceRefreshToken _appServiceRefreshToken;
        private IUriAppService _uriAppService;
        private readonly IEmailSenderService _emailSender;

        public UserController(INotifierAppService notifier, IAppServiceUser appServiceUser, UserManager<ApplicationUser> userManager, ITokenService tokenService, IAppServiceRefreshToken appServiceRefreshToken, IUriAppService uriAppService, IEmailSenderService emailSender) : base(notifier)
        {
            _appServiceUser = appServiceUser;
            _userManager = userManager;
            _tokenService = tokenService;
            _appServiceRefreshToken = appServiceRefreshToken;
            _uriAppService = uriAppService;
            _emailSender = emailSender;
        }

        [HttpGet]
        [Route("get-all-users")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.ADMIN_SYSTEM)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var user = GetUser();
                var result = await _appServiceUser.GetAllUsers();
                return CustomResponse(result);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-home-cards")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHomeDetails()
        {
            try
            {
                var user = GetUser();
                var result = await _appServiceUser.GetHomeDetails(user.Id);
                return CustomResponse(result);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }


        [HttpGet]
        [Route("get-all-users-filter")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.ADMIN_SYSTEM)]
        public async Task<IActionResult> GetAllUsersFilter(string value)
        {
            try
            {
                var user = GetUser();
                var result = await _appServiceUser.GetAllUsersFilter(value);
                return CustomResponse(result);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }

        [HttpGet]
        [Route("get-user-byid")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> GetByIdAsync(string idUser)
        {
            try
            {
                var user = GetUser();
                var result = await _appServiceUser.GetByIdAsync(idUser);
                return CustomResponse(result);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }

        [HttpPut]
        [Route("update-perfil")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.USER)]
        public async Task<IActionResult> UpdatePerfil(UpdatePerfilUserViewModel updatePerfilUserView)
        {
            try
            {
                var result = await _appServiceUser.UpdateAsync(updatePerfilUserView);
                return CustomResponse(result);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }

        [HttpPut]
        [Route("amount-users-dash")]
        [AllowAnonymous]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.ADMIN_SYSTEM)]
        public async Task<IActionResult> TotalUsersAmount()
        {
            try
            {
                var response = await _appServiceUser.TotalUsersAmount();
                return CustomResponse(response);
            }
            catch (Exception e)
            {
                ErrorNotifier("erro", e.Message);
                return CustomResponse();
            }
        }
    }
}
