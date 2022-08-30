using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace PetClub.AppService.AppServices.NotifierAppService
{
    public class NotifierAppService : INotifierAppService
    {
        private List<NotificationMessage> _notifications;

        public NotifierAppService()
        {
            _notifications = new List<NotificationMessage>();
        }
        public List<NotificationMessage> GetNotifications()
        {
            return _notifications;
        }

        public void Handle(NotificationMessage notificationMessage)
        {
            _notifications.Add(notificationMessage);
        }

        public bool Execute<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : class
        {
            var validator = validation.Validate(entity);
            if (validator.IsValid) return true;
            foreach (var item in validator.Errors)
            {
                Handle(new NotificationMessage("", item.ErrorMessage));
            }
            return false;
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }
    }
}
