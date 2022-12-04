using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.ViewModels.Account;

namespace PetClub.Controllers
{
    public abstract class MainController : ControllerBase
    {
        private readonly INotifierAppService _notifier;
        public MainController(INotifierAppService notifier)
        {
            _notifier = notifier;
        }

        protected bool ValidOperation()
        {
            return !_notifier.HasNotification();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (ValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifier.GetNotifications()
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifyInvalidModel(modelState);

            return CustomResponse();
        }

        protected void NotifyInvalidModel(ModelStateDictionary modelState)
        {
            var errors = modelState.Where(x => x.Value.Errors.Count > 0).ToList();
            foreach (var erro in errors)
            {
                ErrorNotifier(erro.Key, erro.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault());
            }
        }

        protected void ErrorNotifier(string property, string message)
        {
            _notifier.Handle(new NotificationMessage(property, message));
        }

        [NonAction]
        protected UserInfo GetUser()
        {
            return new UserInfo()
            {
                Id = User.FindFirstValue("IdUser"),
                Cpf = User.FindFirstValue("Cpf"),
                HasRegistered = User.Identity.IsAuthenticated
            };
        }
    }
}