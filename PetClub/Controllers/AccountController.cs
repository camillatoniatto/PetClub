using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.PaymentMethodAppService;
using PetClub.AppService.AppServices.ServiceRefreshTokenAppService;
using PetClub.AppService.AppServices.UserAppService;
using PetClub.AppService.Extensions;
using PetClub.AppService.Image.Model;
using PetClub.AppService.ViewModels.Account;
using PetClub.AppService.ViewModels.User;
using PetClub.Configurations;
using PetClub.CrossCutting.Identity.Interfaces;
using PetClub.CrossCutting.Identity.Models;
using PetClub.CrossCutting.Identity.ViewModel;
using PetClub.Domain.Entities;
using PetClub.Domain.Extensions;
using PetClub.Utils;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;

namespace PetClub.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountController : MainController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IAppServiceRequestAssociate _appServiceRequestAssociate;
        private readonly IAppServiceUser _appServiceUser;
        private readonly ITokenService _tokenService;
        private readonly IEmailSenderService _emailSender;
        private readonly IAppServiceRefreshToken _appServiceRefreshToken;
        //private readonly IAppServiceAwsS3 _appServiceAwsS3;
        private readonly IAppServicePaymentMethod _appServicePaymentMethod;
        private static IHttpContextAccessor _httpContextAccessor;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService, INotifierAppService notifier, IEmailSenderService emailSender, IHttpContextAccessor httpContextAccessor, 
            IAppServiceUser appServiceUser, IAppServiceRefreshToken appServiceRefreshToken, /*IAppServiceAwsS3 appServiceAwsS3, */IAppServicePaymentMethod appServicePaymentMethod) : base(notifier)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _appServiceUser = appServiceUser;
            _appServiceRefreshToken = appServiceRefreshToken;
            //_appServiceAwsS3 = appServiceAwsS3;
            _appServicePaymentMethod = appServicePaymentMethod;

        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(UserViewModel user)
        {
            user.Cpf = OnlyNumber(user.Cpf);
            user.PhoneNumber = OnlyNumber(user.PhoneNumber);
            var password = user.Cpf;

            if (string.IsNullOrEmpty(user.ZipCode))
            {
                user.AddressName = "";
                user.Number = "";
                user.Complement = "";
                user.Neighborhood = "";
                user.City = "";
                user.State = "";
                user.ZipCode = "";
            }
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var request = await _appServiceUser.GetByCpf(user.Cpf);
            var userData = await ValidRegisterUser(user, user.IsAdmin, user.IsPartner);
            if (userData == null) return CustomResponse();

            IdentityResult result;
            //_userManager.SetUserNameAsync(userData, );
            userData.UserName = user.Cpf;
            password = userData.UserName;

            if (userData.IsActive)
            {
                userData.AddressName = userData.AddressName != null ? userData.AddressName : "";
                userData.Number = userData.Number != null ? userData.Number : "";
                userData.Complement = userData.Complement != null ? userData.Complement : "";
                userData.Neighborhood = userData.Neighborhood != null ? userData.Neighborhood : "";
                userData.City = userData.City != null ? userData.City : "";
                userData.State = userData.State != null ? userData.State : "";
                userData.ZipCode = userData.ZipCode != null ? userData.ZipCode : "";
                result = await _userManager.CreateAsync(userData, password);
            }
            else
            {
                var userById = await _userManager.FindByIdAsync(userData.Id);
                userById.UserName = user.Cpf;
                userById.Email = user.Email;
                userById.Birthdate = user.Birthdate;
                userById.Cpf = user.Cpf;
                userById.FullName = user.FullName;
                userById.PhoneNumber = OnlyNumber(user.PhoneNumber);
                userById.IsAdmin = user.IsAdmin;
                userById.IsPartner = user.IsPartner;
                userById.IsActive = true;
                userById.AddressName = user.AddressName != null ? user.AddressName : "";
                userById.Number = user.Number != null ? user.Number : "";
                userById.Complement = user.Complement != null ? user.Complement : "";
                userById.Neighborhood = user.Neighborhood != null ? user.Neighborhood : "";
                userById.City = user.City != null ? user.City : "";
                userById.State = user.State != null ? user.State : "";
                userById.ZipCode = user.ZipCode != null ? user.ZipCode : "";
                userById.Image = "";
                userById.WriteDate = DateTime.Now.ToBrasilia();
                userData = userById;

                result = await _userManager.UpdateAsync(userById);

                var token = await _userManager.GeneratePasswordResetTokenAsync(userById);
                var passwd = await _userManager.ResetPasswordAsync(userById, token, password);

                var claims = await _userManager.GetClaimsAsync(userData);
                await _userManager.RemoveClaimAsync(userData, claims.FirstOrDefault());

            }

            if (result.Succeeded)
            {
                if (user.IsAdmin)
                {
                    await _userManager.AddClaimAsync(userData, new Claim(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PROFILE_ADMIN));
                }
                else if (user.IsPartner)
                {
                    await _userManager.AddClaimAsync(userData, new Claim(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PROFILE_USER));
                }
                else
                {
                    await _userManager.AddClaimAsync(userData, new Claim(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PROFILE_USER));
                }
                await _signInManager.SignInAsync(userData, false);

                var refresh = Guid.NewGuid().ToString();
                await _appServiceRefreshToken.Create(new RefreshTokenDataViewModel(userData.Id, refresh));

                var identityClaims = new ClaimsIdentity();
                identityClaims.AddClaims(await AddClaimLogin(userData));

                return CustomResponse(_tokenService.GenerateToken(identityClaims, userData, refresh, user.IsAdmin, user.IsPartner));
            }

            return CustomResponse(ModelState);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            //if (!ModelState.IsValid) return CustomResponse(ModelState);
            loginViewModel.UserName = OnlyNumber(loginViewModel.UserName);

            var userData = await _userManager.FindByNameAsync(loginViewModel.UserName);
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user == null || !user.IsActive)
            {
                ErrorNotifier("username", "Usuário não encontrado");
            }
            else
            if (user.IsActive)
            {
                if (loginViewModel.GrantType == "password")
                {
                    var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, true, false);
                    if (result.Succeeded)
                    {
                        var refresh = Guid.NewGuid().ToString();
                        await _appServiceRefreshToken.Create(new RefreshTokenDataViewModel(user.Id, refresh));
                        var identityClaims = new ClaimsIdentity();
                        identityClaims.AddClaims(await AddClaimLogin(user));

                        var resposta = CustomResponse(_tokenService.GenerateToken(identityClaims, userData, refresh, user.IsAdmin, user.IsPartner));
                        return resposta;
                    }
                }
                else if (loginViewModel.GrantType == "refresh_token")
                {
                    var newToken = await _appServiceRefreshToken.ValidateToken(loginViewModel, user.Id);
                    if (string.IsNullOrEmpty(newToken)) { ErrorNotifier("token", "Realizar o login novamente."); return CustomResponse(); }
                    var identityClaims = new ClaimsIdentity();
                    identityClaims.AddClaims(await AddClaimLogin(userData));

                    return CustomResponse(_tokenService.GenerateToken(identityClaims, userData, newToken, user.IsAdmin, user.IsPartner));

                }
                ErrorNotifier("username", "Usuário ou senha inválido");
            }

            return CustomResponse();
        }

        private async Task<ApplicationUser> ValidRegisterUser(UserViewModel user, bool isAdmin, bool isPartner)
        {
            var userExist = await _userManager.FindByNameAsync(user.Cpf);
            if (userExist != null && userExist.IsActive) { ErrorNotifier("Cpf", "Já existe uma conta associada a este Cpf (" + user.Cpf + ")."); return null; }

            if (userExist == null)
            {
                var emailExist = await _userManager.FindByEmailAsync(user.Email);
                if (emailExist != null) { ErrorNotifier("Email", "Já existe uma conta associada a este Email (" + user.Email + ")."); return null; }
            }

            if (userExist != null && !userExist.IsActive)
            {
                var userInactive = new ApplicationUser()
                {
                    Id = userExist.Id,
                    UserName = user.Cpf,
                    Email = user.Email,
                    Cpf = user.Cpf,
                    Birthdate = user.Birthdate,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    IsAdmin = isAdmin,
                    IsPartner = isPartner,
                    IsActive = false,
                    AddressName = user.AddressName,
                    Number = user.Number,
                    Complement = user.Complement,
                    Neighborhood = user.Neighborhood,
                    City = user.City,
                    State = user.State,
                    ZipCode = user.ZipCode,
                    Image = ""
    };

                return userInactive;
            }

            var userData = new ApplicationUser()
            {
                UserName = user.Cpf,
                Email = user.Email,
                Cpf = user.Cpf,
                Birthdate = user.Birthdate,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                IsAdmin = isAdmin,
                IsPartner = isPartner,
                IsActive = true,
                AddressName = user.AddressName,
                Number = user.Number,
                Complement = user.Complement,
                Neighborhood = user.Neighborhood,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode,
                Image = ""
            };

            return userData;
        }

        private async Task<ApplicationUser> VerifyUser(UserViewModel user, bool isAdmin, bool isPartner)
        {
            var userExist = await _userManager.FindByNameAsync(user.Cpf);
            if (userExist != null)
            {
                var userInactive = new ApplicationUser()
                {
                    Id = userExist.Id,
                    UserName = user.Cpf,
                    Email = user.Email,
                    Cpf = user.Cpf,
                    Birthdate = user.Birthdate,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    IsAdmin = isAdmin,
                    IsPartner = isPartner,
                    IsActive = false,
                    AddressName = user.AddressName,
                    Number = user.Number,
                    Complement = user.Complement,
                    Neighborhood = user.Neighborhood,
                    City = user.City,
                    State = user.State,
                    ZipCode = user.ZipCode,
                    Image = ""

                };

                return userInactive;
            }

            var userData = new ApplicationUser()
            {
                UserName = user.Cpf,
                Email = user.Email,
                Cpf = user.Cpf,
                Birthdate = user.Birthdate,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                IsAdmin = isAdmin,
                IsPartner = isPartner,
                IsActive = true,
                AddressName = user.AddressName,
                Number = user.Number,
                Complement = user.Complement,
                Neighborhood = user.Neighborhood,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode,
                Image = ""
            };

            return userData;
        }

        [HttpPut]
        [Route("update-user-admin")]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.ADMIN_SYSTEM)]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAdmin(UserAdminUpdateViewModel model)
        {
            try
            {
                var getUser = await _appServiceUser.GetByIdAsync(model.Id);
                var userData = await _userManager.FindByNameAsync(getUser.Cpf);

                var userById = await _userManager.FindByIdAsync(userData.Id);
                userById.IsAdmin = model.IsAdmin;
                userById.IsPartner = model.IsPartner;
                userData = userById;
                var result = await _userManager.UpdateAsync(userById);

                var claims = await _userManager.GetClaimsAsync(userData);
                await _userManager.RemoveClaimAsync(userData, claims.FirstOrDefault());
                if (result.Succeeded)
                {
                    if (model.IsAdmin)
                    {
                        await _userManager.AddClaimAsync(userData, new Claim(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PROFILE_ADMIN));
                    }
                    else if (model.IsPartner)
                    {
                        await _userManager.AddClaimAsync(userData, new Claim(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PROFILE_PARTNER));
                    }
                    else
                    {
                        await _userManager.AddClaimAsync(userData, new Claim(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PROFILE_USER));
                    }
                }
                

                return CustomResponse(userData);
            }
            catch (Exception e)
            {
                return CustomResponse(e.Message);
            }
        }

        [HttpPost]
        [Route("auth")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthToken()
        {
            var user = GetUser();
            return CustomResponse(user);
        }

        private async Task<IList<Claim>> AddClaimLogin(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim("IdUser", user.Id));
            claims.Add(new Claim("name", user.FullName));
            claims.Add(new Claim("cpf", user.Cpf));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim("admin_system", user.IsAdmin.ToString()));
            claims.Add(new Claim("partner", user.IsPartner.ToString()));
            claims.Add(new Claim("user", "user"));
            return claims;

        }
        private static readonly Regex digitsOnly = new Regex(@"[^\d]");
        public static string OnlyNumber(string document)
        {
            return digitsOnly.Replace(document, "");
        }
    }
}
