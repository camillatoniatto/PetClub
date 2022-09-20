using AutoMapper.Execution;
using Datletica.Api.Controllers;
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
            var request = await _appServiceUser.GetByCpf(user.Cpf);
            var userData = await ValidRegisterUser(user, false, false);
            if (userData == null) return CustomResponse();

            string imageUrl = string.Empty;
            string folder = "PetClub";
            //{
            //    var img = new EventImage();
            //    img.Value = user.Image;
            //    user.UrlImage = await _appServiceAwsS3.UploadImageToS3(img, folder);
            //}

            IdentityResult result;
            //_userManager.SetUserNameAsync(userData, );
            userData.UserName = user.Cpf;

            if (userData.IsActive)
                result = await _userManager.CreateAsync(userData, user.Password);
            else
            {
                var userById = await _userManager.FindByIdAsync(userData.Id);
                userById.UserName = user.Cpf;
                userById.Email = user.Email;
                userById.Birthdate = user.Birthdate;
                userById.Cpf = user.Cpf;
                userById.FullName = user.FullName;
                userById.PhoneNumber = user.PhoneNumber;
                userById.IsAdmin = false;
                userById.IsPartner = false;
                userById.IsActive = true;
                userById.AddressName = user.AddressName;
                userById.Number = user.Number;
                userById.Complement = user.Complement;
                userById.Neighborhood = user.Neighborhood;
                userById.City = user.City;
                userById.State = user.State;
                userById.ZipCode = user.ZipCode;
                userById.Image = "";
                userData = userById;

                result = await _userManager.UpdateAsync(userById);

                var token = await _userManager.GeneratePasswordResetTokenAsync(userById);
                var passwd = await _userManager.ResetPasswordAsync(userById, token, user.Password);

                var claims = await _userManager.GetClaimsAsync(userData);
                await _userManager.RemoveClaimAsync(userData, claims.FirstOrDefault());

            }

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(userData, new Claim(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PROFILE_USER));
                await _signInManager.SignInAsync(userData, false);

                var refresh = Guid.NewGuid().ToString();
                await _appServiceRefreshToken.Create(new RefreshTokenDataViewModel(userData.Id, refresh));

                var identityClaims = new ClaimsIdentity();
                identityClaims.AddClaims(await AddClaimLogin(userData));
                //try
                //{
                //    await _emailSender.SendEmailAsync(user.Email, "Boas vindas da Datlética!", Emails.PreRegisterUser(user.FullName));
                //}
                //catch (Exception)
                //{
                //    ErrorNotifier("ErrorSendEmail", "Sua conta foi cadastrada com sucesso! Porém não foi possível concluir o envio email para '" + user.Email + "'");
                //}

                return CustomResponse(_tokenService.GenerateToken(identityClaims, userData, refresh, false, false));
            }

            return CustomResponse(ModelState);
        }


        [Route("register-user-admin")]
        [HttpPost]
        //[ClaimsAuthorize(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.ADMIN_SYSTEM)]
        [AllowAnonymous]

        public async Task<ActionResult> RegisterUserAdmin([FromBody] UserViewModel user)
        {
            var userLogin = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            user.Cpf = OnlyNumber(user.Cpf);
            var userData = await ValidRegisterUser(user, true, false);
            if (userData == null) return CustomResponse();
            var result = await _userManager.CreateAsync(userData, userData.UserName);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(userData, new Claim(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PROFILE_ADMIN));
                await _emailSender.SendEmailAsync(user.Email, "Diretoria Datlética", Emails.RegisterAdmin("https://painel.datletica.com.br/", userData.FullName, userData.Cpf));
                return CustomResponse();
            }

            foreach (var error in result.Errors)
            {
                ErrorNotifier("Cpf", error.Description);
            }
            return CustomResponse(ModelState);
        }

        [HttpPost]
        [Route("register-user-partner")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterUserAssociate([FromBody] UserViewModel user)
        {
            var userLogin = GetUser();
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            user.Cpf = OnlyNumber(user.Cpf);
            user.Password = user.Cpf;
            var userData = await ValidRegisterUser(user, false, true);
            if (userData == null) return CustomResponse();
            IdentityResult result;
            if (userData.IsActive)
                result = await _userManager.CreateAsync(userData, user.Password);
            else
            {
                var userById = await _userManager.FindByIdAsync(userData.Id);
                userById.UserName = user.Cpf;
                userById.Email = user.Email;
                userById.Birthdate = user.Birthdate;
                userById.Cpf = user.Cpf;
                userById.FullName = user.FullName;
                userById.PhoneNumber = user.PhoneNumber;
                userById.IsAdmin = false;
                userById.IsPartner = true;
                userById.IsActive = true;
                userById.AddressName = user.AddressName;
                userById.Number = user.Number;
                userById.Complement = user.Complement;
                userById.Neighborhood = user.Neighborhood;
                userById.City = user.City;
                userById.State = user.State;
                userById.ZipCode = user.ZipCode;
                userById.Image = "";
                userData = userById;
                result = await _userManager.UpdateAsync(userById);
                var claims = await _userManager.GetClaimsAsync(userData);
                await _userManager.RemoveClaimAsync(userData, claims.FirstOrDefault());
                // return await UserController.UpdateAssociate(new UserAssociatedUpdateViewModel(userData.Id, user.FullName, user.Cpf, user.Email, user.PhoneNumber, null, user.RegisterAcademic, user.AccessToAll, user.AccessToFinancial, user.AccessToProducts, user.AccessToEvents, user.AccessToCommunity, user.IsAssociate, user.IsAthlete, user.IsFan, user.IsDirector, true));
            }

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(userData, new Claim(AuthorizeSetup.CLAIM_TYPE_OCCUPATION, AuthorizeSetup.PROFILE_PARTNER));
                try
                {
                    await _emailSender.SendEmailAsync(user.Email, "Boas vindas da Datlética!", Emails.PreRegisterUser(user.FullName));
                }
                catch (Exception)
                {
                    ErrorNotifier("ErrorSendEmail", "Usuário cadastrado com sucesso! Porém não foi possível concluir o envio email para '" + user.Email + "'");
                }
                return CustomResponse();
            }

            foreach (var error in result.Errors)
            {
                ErrorNotifier("Cpf", error.Description);
            }
            return CustomResponse(ModelState);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            //if (!ModelState.IsValid) return CustomResponse(ModelState);
            var userData = await _userManager.FindByNameAsync(loginViewModel.UserName);
            var user = await _appServiceUser.GetByCpf(loginViewModel.UserName);
            if (user == null || !user.IsActive)
            {
                ErrorNotifier("username", "Usuário não encontrado");
            }
            else
            if (user.IsActive)
            {
                if (loginViewModel.GrantType == "password")
                {
                    var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        var refresh = Guid.NewGuid().ToString();
                        await _appServiceRefreshToken.Create(new RefreshTokenDataViewModel(user.Id, refresh));
                        var identityClaims = new ClaimsIdentity();

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


        [HttpGet]
        [AllowAnonymous]
        [Route("send-password-reset-link")]
        public async Task<ActionResult> SendPasswordResetLink(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null || !user.IsActive)
            {
                ErrorNotifier("Cpf", "Esse cpf não existe ou é inválido.");
                return CustomResponse();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string resetLink = "";
            string email = "";
            try
            {

                //resetLink =  Url.Action("ResetPassword", "Account", new {version = "1.0", user.UserName}, _httpContextAccessor.HttpContext.Request.Scheme);
                resetLink = "http://painel.datletica.com.br/resetpassword/" + user.UserName + "/" + HttpUtility.HtmlEncode(token);
                email = Emails.PasswordReset(resetLink, user.FullName);
                await _emailSender.SendEmailAsync(user.Email, "Datlética - Alteração de senha", email);

            }
            catch (Exception e)
            {
                return CustomResponse(e.Message);
            }

            return CustomResponse("Link para alteração de senha foi enviado com sucesso no seu email");
        }

        [HttpPost]
        [Route("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {

            try
            {
                var user = await _userManager.FindByNameAsync(resetPasswordViewModel.UserName);
                if (!user.IsActive)
                    ErrorNotifier("Reset", "Usuário não encontrado no sistema");
                else
                {
                    var result =
                        await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Token,
                            resetPasswordViewModel.Password);

                    if (result.Succeeded)
                    {
                        var email = Emails.PasswordResetConfirmed(user.FullName);
                        await _emailSender.SendEmailAsync(user.Email, "Datlética - Alteração de senha", email);
                        return CustomResponse("Senha alterada com sucesso!");
                    }
                    else
                    {
                        //ErrorNotifier("Reset", result.Errors.ElementAt(0).Description);
                        ErrorNotifier("Reset", "O tempo de troca de senha foi expirado ou token já foi utilizado. Caso não tenha conseguido trocar a senha, por favor reenvie novamente um email de alteração de senha.");

                    }
                }
                return CustomResponse();
            }
            catch (Exception)
            {
                return CustomResponse();
            }
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
            catch (Exception)
            {
                return CustomResponse();
            }
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
