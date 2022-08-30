using Datletica.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.UriAppService.Interfaces;
using PetClub.AppService.AppServices.UserAppService;
using PetClub.AppService.AppServices.ServiceRefreshTokenAppService;
using PetClub.CrossCutting.Identity.Interfaces;
using PetClub.CrossCutting.Identity.Models;

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
    }
}
