using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.NotifierAppService
{
    public interface INotifierAppService
    {
        bool HasNotification();
        List<NotificationMessage> GetNotifications();
        void Handle(NotificationMessage notificationMessage);
        bool Execute<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : class;
    }
}
